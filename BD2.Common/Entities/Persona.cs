using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public int NumeroAfiliado { get; set; }
        public Domicilio Domicilio { get; set; }
        public ObraSocial ObraSocial { get; set; }

    }
}
