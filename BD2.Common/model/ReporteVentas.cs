using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.model
{
    public class VentasReporte
    {
        public List<VentaCadena> TotalVentasCadena { get; set; }
        public List<VentaSucursal> VentasPorSucursal { get; set; }
    }

    public class VentaCadena
    {
        public string _id { get; set; }
        public int TotalCantidadVentas { get; set; }
    }

    public class VentaSucursal
    {
        public SucursalNombreClass _id { get; set; }
        public int TotalCantidadVentas { get; set; }
    }

    public class SucursalNombreClass
    {
        public int SucursalNombre { get; set; }
    }
}
