using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string Calle {  get; set; }
        public Localidad Localidad { get; set; }
        public int Altura { get; set; }
        public int NumeroSucursal { get; set; }
    }
}
