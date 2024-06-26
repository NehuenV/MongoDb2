﻿using BD2.Common.Entities;
using BD2.Common.model;
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
        Task<VentasReporte> punto1(DateTime fechaDesde, DateTime fechaHasta);
        Task<VentaPorObraSocial> punto2(DateTime fechaDesde, DateTime fechaHasta);
        Task<Cobranza> punto3(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<CantidadProductos>> punto4(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<RankingVentaProducto>> punto5();
        Task<List<RankingCantidadProductos>> punto6();
        Task<List<ClienteCompra>> punto7();
        Task<List<ClienteSucursalCompra>> punto8();
    }
}
