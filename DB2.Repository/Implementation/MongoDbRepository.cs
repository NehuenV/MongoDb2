using BD2.Common;
using BD2.Common.Entities;
using BD2.Common.model;
using DB2.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB2.Repository.Implementation
{
    public class MongoDbRepository : IMongoDbRepository
    {
        private readonly IMongoDatabase _dataBase;
        private readonly IMongoCollection<Factura> _facturaCollection;
        public MongoDbRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> options)
        {
            _dataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
            _facturaCollection = _dataBase.GetCollection<Factura>("Facturas");
        }

        public async Task<bool> AgregarData(List<Factura> lista)
        {
            try
            {
                var collection = _dataBase.GetCollection<Factura>("Facturas");
                await collection.InsertManyAsync(lista);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<Factura>> ConsultarData()
        {
            try
            {

                var result = await _facturaCollection.Find(new BsonDocument()).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Factura>();
            }
        }

        public async Task<VentasReporte> punto1(DateTime fechaDesde, DateTime fechaHasta)
        {
            var matchStage = new BsonDocument("$match", new BsonDocument
            {
                { "FechaHora", new BsonDocument
                    {
                        { "$gte", new BsonDateTime(new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc)) },
                        { "$lte", new BsonDateTime(new DateTime(2024, 6, 30, 23, 59, 59, 999, DateTimeKind.Utc)) }
                    }
                }
            });

            var facetStage = new BsonDocument("$facet", new BsonDocument
            {
                { "TotalVentasCadena", new BsonArray
                    {
                        new BsonDocument("$group", new BsonDocument
                        {
                            { "_id", BsonNull.Value },
                            { "TotalCantidadVentas", new BsonDocument("$sum", 1) }
                        })
                    }
                },
                { "VentasPorSucursal", new BsonArray
                    {
                        new BsonDocument("$group", new BsonDocument
                        {
                            { "_id", new BsonDocument("SucursalNombre", "$Sucursal.NumeroSucursal") },
                            { "TotalCantidadVentas", new BsonDocument("$sum", 1) }
                        }),
                        new BsonDocument("$sort", new BsonDocument("TotalCantidadVentas", -1))
                    }
                }
            });

            var pipeline = new[] { matchStage, facetStage };
            var cursor = await _facturaCollection.AggregateAsync<BsonDocument>(pipeline);
            var ventasReporte = new VentasReporte();
            await cursor.ForEachAsync(doc =>
            {
                if (doc.TryGetValue("TotalVentasCadena", out var totalVentasCadena))
                {
                    ventasReporte.TotalVentasCadena = totalVentasCadena.AsBsonArray
                        .Select(v => BsonSerializer.Deserialize<VentaCadena>(v.AsBsonDocument))
                        .ToList();
                }
                if (doc.TryGetValue("VentasPorSucursal", out var ventasPorSucursal))
                {
                    ventasReporte.VentasPorSucursal = ventasPorSucursal.AsBsonArray
                        .Select(v => BsonSerializer.Deserialize<VentaSucursal>(v.AsBsonDocument))
                        .ToList();
                }
            });
            return ventasReporte;
        }
        // punto 2
        public async Task<VentaPorObraSocial> punto2(DateTime fechaDesde, DateTime fechaHasta)
        {
            var matchStage = new BsonDocument("$match", new BsonDocument
            {
                { "FechaHora", new BsonDocument
                    {
                        { "$gte", new BsonDateTime(new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc)) },
                        { "$lte", new BsonDateTime(new DateTime(2024, 6, 30, 23, 59, 59, 999, DateTimeKind.Utc)) }
                    }
                }
            });

            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument
                    {
                        { "ObraSocial", new BsonDocument("$ifNull", new BsonArray { "$Cliente.ObraSocial.Nombre", "Privado" }) }
                    }
                },
                { "TotalCantidadVentas", new BsonDocument("$sum", 1) }
            });

            var pipeline = new[] { matchStage, groupStage };

            var cursor = await _facturaCollection.AggregateAsync<BsonDocument>(pipeline);
            VentaPorObraSocial ventaPorObraSocial = new VentaPorObraSocial();
            await cursor.ForEachAsync(doc =>
            {
                ventaPorObraSocial = BsonSerializer.Deserialize<VentaPorObraSocial>(doc);
            });
            return ventaPorObraSocial;
        }

        // punto 3
        public async Task<Cobranza> punto3(DateTime fechaDesde, DateTime fechaHasta)
        {
            var matchStage = new BsonDocument("$match", new BsonDocument
            {
                { "FechaHora", new BsonDocument
                    {
                        { "$gte", new BsonDateTime(fechaDesde)},
                        { "$lte", new BsonDateTime(fechaHasta) }
                    }
                }
            });

            var facetStage = new BsonDocument("$facet", new BsonDocument
            {
                {
                    "TotalCobranzaCadena", new BsonArray
                    {
                        new BsonDocument("$group", new BsonDocument
                        {
                            { "_id", "Tipo" },
                            { "TotalCobranza", new BsonDocument("$sum", new BsonDocument("$toDouble", "$TotalVenta")) }
                        }),
                        new BsonDocument("$project", new BsonDocument
                        {
                            { "_id", 0 },
                            { "TotalCobranza", 1 }
                        })
                    }
                },
                {
                    "CobranzaPorSucursal", new BsonArray
                    {
                        new BsonDocument("$group", new BsonDocument
                        {
                            { "_id", "$Sucursal.NumeroSucursal" },
                            { "TotalCobranza", new BsonDocument("$sum", new BsonDocument("$toDouble", "$TotalVenta")) }
                        }),
                        new BsonDocument("$project", new BsonDocument
                        {
                            { "_id", 0 },
                            { "SucursalNumero", "$_id" },
                            { "TotalCobranza", 1 }
                        })
                    }
                }
            });

            var pipeline = new[] { matchStage, facetStage };
            var cursor = await _facturaCollection.AggregateAsync<BsonDocument>(pipeline);

            // Mapea los resultados a las clases definidas
            var result = await cursor.FirstOrDefaultAsync();
            var totalCobranzaCadenaArray = result["TotalCobranzaCadena"].AsBsonArray;
            var totalCobranzaCadena = new List<TotalCobranzaCadena>();

            foreach (var item in totalCobranzaCadenaArray)
            {
                var total = BsonSerializer.Deserialize<TotalCobranzaCadena>(item.AsBsonDocument);
                totalCobranzaCadena.Add(total);
            }

            var cobranzaPorSucursalArray = result["CobranzaPorSucursal"].AsBsonArray;
            var cobranzaPorSucursal = new List<CobranzaPorSucursal>();

            foreach (var item in cobranzaPorSucursalArray)
            {
                var cobranza = BsonSerializer.Deserialize<CobranzaPorSucursal>(item.AsBsonDocument);
                cobranzaPorSucursal.Add(cobranza);
            }

            var cobranzaResult = new Cobranza { cobranzaPorSucursal = cobranzaPorSucursal, totalCobranzaCadena = totalCobranzaCadena };

            return cobranzaResult;
        }

        // punto 4
        public async Task<List<CantidadProductos>> punto4(DateTime fechaDesde, DateTime fechaHasta)
        {
            FilterDefinition<BsonDocument> filtroFechas = Builders<BsonDocument>.Filter.And(
             Builders<BsonDocument>.Filter.Gte("FechaHora", fechaDesde),
             Builders<BsonDocument>.Filter.Lte("FechaHora", fechaHasta));
            var aggregationPipeline = new BsonDocument[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "FechaHora", new BsonDocument
                    {
                        { "$gte", fechaDesde },
                        { "$lte", fechaHasta }
                    }
                }
            }),
            new BsonDocument("$unwind", "$DetalleFactura"),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$DetalleFactura.Producto.TipoProducto.Nombre" },
                { "totalVendido", new BsonDocument("$sum", "$DetalleFactura.Cantidad") }
            })
        };


            // Ejecutar la agregación
            var aggregationResult = await _facturaCollection.AggregateAsync<BsonDocument>(aggregationPipeline);
            var result = new List<CantidadProductos>();
            // Procesar los resultados
            await aggregationResult.ForEachAsync(document =>
            {
                var tipoProducto = document["_id"].AsString;
                var totalVendido = document["totalVendido"].AsInt32;
                result.Add(new CantidadProductos { TipoProducto = tipoProducto, CantidadProducto = totalVendido });
            });
            return result;
        }
        public async Task<List<RankingVentaProducto>> punto5()
        {
            var pipeline = new List<BsonDocument>
            {
                new BsonDocument("$unwind", "$DetalleFactura"),

                new BsonDocument("$addFields", new BsonDocument
                {
                    { "DetalleFactura.PrecioVentaNumeric", new BsonDocument("$toDouble", "$DetalleFactura.PrecioVenta") },
                    { "DetalleFactura.CantidadNumeric", new BsonDocument("$toInt", "$DetalleFactura.Cantidad") }
                }),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            { "producto", "$DetalleFactura.Producto.Nombre" },
                            { "sucursal", "$Sucursal.NumeroSucursal" }
                        }
                    },
                    { "totalVendido", new BsonDocument("$sum", new BsonDocument("$multiply", new BsonArray
                        {
                            "$DetalleFactura.PrecioVentaNumeric",
                            "$DetalleFactura.Cantidad"
                        })
                    )}
                }),
                new BsonDocument("$sort", new BsonDocument("totalVendido", -1))
            };
            var aggregationResult = await _facturaCollection.AggregateAsync<BsonDocument>(pipeline);
            var result = new List<RankingVentaProducto>();
            await aggregationResult.ForEachAsync(document =>
            {
                var producto = document["_id"]["producto"].AsString;
                var sucursal = document["_id"]["sucursal"].AsInt32;
                var totalVendido = document["totalVendido"].AsDouble;
                result.Add(new RankingVentaProducto { montoVenta = totalVendido, NumeroSucursal = sucursal ,Producto = producto});
            });
            return result;
        }
        public async Task<List<RankingCantidadProductos>> punto6()
        {
            var pipeline = new List<BsonDocument>
        {
            new BsonDocument("$unwind", "$DetalleFactura"),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument
                    {
                        { "producto", "$DetalleFactura.Producto.Nombre" },
                        { "sucursal", "$Sucursal.NumeroSucursal" }
                    }
                },
                { "cantidadVendida", new BsonDocument("$sum", "$DetalleFactura.Cantidad") }
            }),
            new BsonDocument("$sort", new BsonDocument("cantidadVendida", -1))
        };

            var aggregationResult = await _facturaCollection.AggregateAsync<BsonDocument>(pipeline);
            var result = new List<RankingCantidadProductos>();
            await aggregationResult.ForEachAsync(document =>
            {
                var producto = document["_id"]["producto"].AsString;
                var sucursal = document["_id"]["sucursal"].AsInt32;
                var cantidadVendida = document["cantidadVendida"].AsInt32;
                result.Add(new RankingCantidadProductos { CantidadVenta = cantidadVendida, NumeroSucursal = sucursal, Producto = producto });
            });
            return result;
        }
        public async Task<List<ClienteCompra>> punto7()
        {
            var pipeline = new BsonDocument[]
            {
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "ClienteId", "$Cliente.IdPersona" },
                                { "Nombre", "$Cliente.Nombre" },
                                { "Apellido", "$Cliente.Apellido" }
                            }
                        },
                        { "TotalCompras", new BsonDocument("$sum", new BsonDocument("$toDouble", "$TotalVenta")) }
                    }),
                    new BsonDocument("$sort", new BsonDocument("TotalCompras", -1))
            };
            var result = _facturaCollection.Aggregate<BsonDocument>(pipeline);
            var clientesCompras = new List<ClienteCompra>();
            await result.ForEachAsync(doc =>
            {
                var clienteCompra = new ClienteCompra
                {
                    ClienteId = doc["_id"]["ClienteId"].AsInt32,
                    Nombre = doc["_id"]["Nombre"].AsString,
                    Apellido = doc["_id"]["Apellido"].AsString,
                    TotalCompras = doc["TotalCompras"].ToDouble()
                };
                clientesCompras.Add(clienteCompra);
            });
            return clientesCompras;
        }

        public async Task<List<ClienteSucursalCompra>> punto8()
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            
                            { "Nombre", "$Cliente.Nombre" },
                            { "Apellido", "$Cliente.Apellido" },
                            
                            { "NumeroSucursal", "$Sucursal.NumeroSucursal" }
                        }
                    },
                    { "TotalCompras", new BsonDocument("$sum", new BsonDocument("$toDouble", "$TotalVenta")) }
                }),
                new BsonDocument("$sort", new BsonDocument("TotalCompras", -1))
            };
            var result = _facturaCollection.Aggregate<BsonDocument>(pipeline);
            var clientesSucursalesCompras = new List<ClienteSucursalCompra>();

            await result.ForEachAsync(doc =>
            {
                var clienteSucursalCompra = new ClienteSucursalCompra
                {
                    Nombre = doc["_id"]["Nombre"].AsString,
                    Apellido = doc["_id"]["Apellido"].AsString,
                    SucursalId = doc["_id"]["NumeroSucursal"].AsInt32,
                    TotalCompras = doc["TotalCompras"].ToDouble()
                };
                clientesSucursalesCompras.Add(clienteSucursalCompra);
            });
            return clientesSucursalesCompras;
        }
    }
}
