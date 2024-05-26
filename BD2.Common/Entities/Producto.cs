using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public TipoProducto TipoProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } 
        public Laboratorio Laboratorio { get; set; }
        public int CodigoNumerico { get; set; }
        public decimal Precio { get; set; }

    }
}
