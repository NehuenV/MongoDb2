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
                return false;
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
        //public async Task<dynamic> ConsultarAgrupadoEntreFechas()
        //{
        //    var collection = _dataBase.GetCollection<BsonDocument>("Facturas"); // Reemplaza con el nombre de tu colección

        //    var filtro = Builders<BsonDocument>.Filter.Eq("Sucursal.NumeroSucursal", 2);
        //    var resultados = await collection.Find(filtro).ToListAsync();

        //    var facturas = new List<Factura>();
        //    foreach (var documento in resultados)
        //    {
        //        var factura = BsonSerializer.Deserialize<Factura>(documento);
        //        facturas.Add(factura);
        //    }

        //    return facturas;
        //}

        // punto 1 

        public async Task<List<ReporteVentas>> punto1(DateTime fechaDesde, DateTime fechaHasta)
        {
            var filtro = Builders<Factura>.Filter.Gte(f => f.FechaHora, fechaDesde) &
                         Builders<Factura>.Filter.Lte(f => f.FechaHora, fechaHasta);

            var ventasPorSucursales = await _facturaCollection.Aggregate()
                .Match(filtro)
                .Group(f => new { f.Sucursal.NumeroSucursal, f.Sucursal.Localidad.Nombre }, g => new
                {
                    IdSucursal = g.Key.NumeroSucursal,
                    NombreSucursal = g.Key.Nombre,
                    TotalVentaSucursal = g.Select(x => x.DetalleFactura.Sum(f => f.Cantidad))
                })
                .ToListAsync();
            return ventasPorSucursales.Select(v => new ReporteVentas
            {
                IdSucursal = v.IdSucursal,
                NombreSucursal = v.NombreSucursal,
                TotalVentas = v.TotalVentaSucursal.Sum()
            }).ToList();
        }
        // punto 2
        public async Task<List<VentasPorSucursalYObraSocial>> punto2(DateTime fechaDesde, DateTime fechaHasta)
        {
            var filtro = Builders<Factura>.Filter.Gte(f => f.FechaHora, fechaDesde) &
                         Builders<Factura>.Filter.Lte(f => f.FechaHora, fechaHasta);

            var ventasPorSucursales = await _facturaCollection.Aggregate()
                .Match(filtro)
                .Group(f => new { f.Sucursal.NumeroSucursal, f.Sucursal.Localidad.Nombre, ObraSocial = f.Cliente.ObraSocial.Nombre }, g => new
                {
                    IdSucursal = g.Key.NumeroSucursal,
                    NombreSucursal = g.Key.Nombre,
                    ObraSocial = g.Key.ObraSocial,
                    TotalVentaSucursal = g.Select(x => x.DetalleFactura.Sum(f => f.Cantidad))
                })


                .ToListAsync();

            return ventasPorSucursales.Select(v => new VentasPorSucursalYObraSocial
            {
                IdSucursal = v.IdSucursal,
                NombreSucursal = v.NombreSucursal,
                TotalVentas = v.TotalVentaSucursal.Sum(),
                ObraSocial = v.ObraSocial
            }).OrderBy(x => x.ObraSocial).ToList();
        }

        // punto 3
        public async Task<List<VentasPorSucursal>> punto3(DateTime fechaDesde, DateTime fechaHasta)
        {
            var filtro = Builders<Factura>.Filter.Gte(f => f.FechaHora, fechaDesde) &
             Builders<Factura>.Filter.Lte(f => f.FechaHora, fechaHasta);

            var ventasPorSucursales = await _facturaCollection.Aggregate()
                .Match(filtro)
                .Group(f => new { f.Sucursal.NumeroSucursal, f.Sucursal.Localidad.Nombre }, g => new
                {
                    IdSucursal = g.Key.NumeroSucursal,
                    NombreSucursal = g.Key.Nombre,
                    TotalVentaSucursal = g.Select(x => x.TotalVenta)
                })
                .ToListAsync();
            return ventasPorSucursales.Select(v => new VentasPorSucursal
            {
                IdSucursal = v.IdSucursal,
                NombreSucursal = v.NombreSucursal,
                TotalVentas = v.TotalVentaSucursal.Sum()
            }).ToList();
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


    }
}
