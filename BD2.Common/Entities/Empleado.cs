using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class Empleado
    {
        public int Cuil {  get; set; }
        public bool Encargado { get; set; }
        public Persona Persona { get; set; }
        public Sucursal Sucursal { get; set; }

    }
}
