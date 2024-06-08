using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.model
{
    public class TotalCobranzaCadena
    {
        public decimal TotalCobranza { get; set; }
    }

    public class CobranzaPorSucursal
    {
        public decimal TotalCobranza { get; set; }
        public int SucursalNumero { get; set; }
    }
    public class Cobranza
    {
        public List<TotalCobranzaCadena> totalCobranzaCadena { get; set; }
        public List<CobranzaPorSucursal> cobranzaPorSucursal { get; set; }
    }
}
