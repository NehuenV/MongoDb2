using BD2.Common;
using BD2.Common.Entities;
using DB2.Repository.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
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
        public MongoDbRepository(IMongoClient mongoClient, IOptions<MongoDBSettings> options) 
        {
            _dataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
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
                var collection = _dataBase.GetCollection<Factura>("Facturas");
                var result= await collection.Find(new BsonDocument()).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Factura>();
            }
        }
    }
}
