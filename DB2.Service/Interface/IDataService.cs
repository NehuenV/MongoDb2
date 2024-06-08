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
        Task<VentasReporte> punto1(DateTime from, DateTime to);
        Task<VentaPorObraSocial> punto2(DateTime fechaDesde, DateTime fechaHasta);
        Task<Cobranza> punto3(DateTime from, DateTime to);
        Task<List<CantidadProductos>> punto4(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<RankingVentaProducto>> punto5();
        Task<List<RankingCantidadProductos>> punto6();
        Task<List<ClienteCompra>> punto7();
        Task<List<ClienteSucursalCompra>> punto8();
    }
}
