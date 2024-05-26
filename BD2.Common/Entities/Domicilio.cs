using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class Domicilio
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public  int  Numero { get; set; }
        public  Localidad Localidad { get; set; }
        public Provincia Provincia { get; set; }
    }
}
