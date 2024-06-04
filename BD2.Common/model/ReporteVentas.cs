using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.model
{
    public class ReporteVentas
    {
        public ReporteVentas()
        {

        }
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public int TotalVentas { get; set; }


    }
}
