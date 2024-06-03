using BD2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DB2.Repository.Implementation.MongoDbRepository;

namespace DB2.Repository.Interface
{
    public interface IMongoDbRepository
    {
        Task<bool> AgregarData(List<Factura> lista);
        Task<List<Factura>> ConsultarData();
        Task<List<VentasPorSucursal>> ObtenerVentasPorSucursalesAsync(DateTime fechaDesde, DateTime fechaHasta);
        Task<decimal> ObtenerTotalVentasAsync(DateTime fechaDesde, DateTime fechaHasta);
    }
}
