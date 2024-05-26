using BD2.Common.Entities;
using DB2.Repository.Interface;
using DB2.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB2.Service.Implementation
{
    public class DataService: IDataService
    {
        private readonly IMongoDbRepository _mongoDbRepository;
        public DataService(IMongoDbRepository mongoDbRepository) 
        {
            _mongoDbRepository = mongoDbRepository;
        }
        public async Task<bool> AgregarData()
        {
            try
            {
                var facturas = new List<Factura>();
                var result = await _mongoDbRepository.AgregarData(facturas);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Factura>> ConsultarData()
        {
            try
            {
                var result =await _mongoDbRepository.ConsultarData();
                return result;
            }catch (Exception)
            {
                throw;
            }
        }
    }
}
