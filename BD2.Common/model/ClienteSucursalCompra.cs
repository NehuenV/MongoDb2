using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.model
{
    public class ClienteSucursalCompra
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int SucursalId { get; set; }
        public double TotalCompras { get; set; }
    }
}
