using BD2.Common.Entities;
using BD2.Common.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB2.Service.Interface
{
    public interface IDataService
    {
        Task<bool> AgregarData(int cantidad);
        Task<List<Factura>> ConsultarData();
        Task<TotalSucursal<ReporteVentas>> punto1(DateTime from, DateTime to);
        Task<TotalSucursal<VentasPorSucursalYObraSocial>> punto2(DateTime fechaDesde, DateTime fechaHasta);
        Task<TotalSucursal<VentasPorSucursal>> punto3(DateTime from, DateTime to);
        Task<List<CantidadProductos>> punto4(DateTime fechaDesde, DateTime fechaHasta);
    }
}
