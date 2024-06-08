using BD2.Common.Entities;
using BD2.Common.model;
using DB2.Repository.Interface;
using DB2.Service.Interface;
using MongoDB.Driver;
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
        public async Task<VentasReporte> punto1(DateTime from, DateTime to)
        {
            try
            {
                var totalVentas = await _mongoDbRepository.punto1(from, to);
                return totalVentas;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<VentaPorObraSocial> punto2(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var totalVentas = await _mongoDbRepository.punto2(fechaDesde, fechaHasta);
                return totalVentas;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Cobranza> punto3(DateTime from, DateTime to)
        {
            try
            {
                var totalVentas = await _mongoDbRepository.punto3(from, to);
                return totalVentas;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CantidadProductos>> punto4(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var totalVentas = await _mongoDbRepository.punto4(fechaDesde, fechaHasta);
                return totalVentas;// new TotalSucursal<VentasPorSucursalYObraSocial>(totalVentas, totalVentas.Sum(f => f.TotalVentas));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<RankingVentaProducto>> punto5()
        {
            try
            {
               var result = await _mongoDbRepository.punto5();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<RankingCantidadProductos>> punto6()
        {
            try
            {
                var result = await _mongoDbRepository.punto6();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ClienteCompra>> punto7()
        {
            try
            {
                var result = await _mongoDbRepository.punto7();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ClienteSucursalCompra>> punto8()
        {
            try
            {
                var result = await _mongoDbRepository.punto8();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
