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
        public async Task<bool> AgregarData(int cantidad)
        {
            try
            {
                var facturas = await Builder.GetFacturasAsync(cantidad);
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
        public async Task<dynamic> ConsultarAgrupadoEntreFechas()
        {
            try
            {
                var totalVentas = await _mongoDbRepository.ObtenerVentasPorSucursalesAsync(DateTime.Parse("2024/05/01"), DateTime.Parse("2024/06/30"));
                return totalVentas;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
