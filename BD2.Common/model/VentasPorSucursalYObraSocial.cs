using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.model
{
    public class VentaPorObraSocial
    {
        public ObraSocial ObraSocial { get; set; }
        public int TotalCantidadVentas { get; set; }
    }

    public class ObraSocial
    {
        public string Nombre { get; set; }
    }
}
