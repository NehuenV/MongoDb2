using BD2.Common;
using BD2.Common.Entities;
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
                
                var result= await _facturaCollection.Find(new BsonDocument()).ToListAsync();
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
        public class VentasPorSucursal
        {
            public int IdSucursal { get; set; }
            public string NombreSucursal { get; set; }
            public decimal TotalVentas { get; set; }
        }
        public async Task<decimal> ObtenerTotalVentasAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var filtro = Builders<Factura>.Filter.Gte(f => f.FechaHora, fechaDesde) &
                         Builders<Factura>.Filter.Lte(f => f.FechaHora, fechaHasta);

            var resultado = await _facturaCollection.Aggregate()
                       .Match(filtro)
                       .Group(
                           f => f.Sucursal.NumeroSucursal,
                           g => new
                           {
                               NumeroSucursal = g.Key,
                               TotalVentas = g.Sum(x => x.TotalVenta)
                           })
                       .ToListAsync();

            return  0;
        }
        public async Task<List<VentasPorSucursal>> ObtenerVentasPorSucursalesAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var filtro = Builders<Factura>.Filter.Gte(f => f.FechaHora, fechaDesde) &
                         Builders<Factura>.Filter.Lte(f => f.FechaHora, fechaHasta);

            var ventasPorSucursales = await _facturaCollection.Aggregate()
                .Match(filtro)
                .Group(f => new { f.Sucursal.NumeroSucursal, f.Sucursal.Localidad.Nombre }, g => new
                {
                    IdSucursal = g.Key.NumeroSucursal,
                    NombreSucursal = g.Key.Nombre,
                    TotalVentas = g.Sum(f => f.TotalVenta)
                })
                .ToListAsync();

            return ventasPorSucursales.Select(v => new VentasPorSucursal
            {
                IdSucursal = v.IdSucursal,
                NombreSucursal = v.NombreSucursal,
                TotalVentas = v.TotalVentas
            }).ToList();
        }

    }
}
