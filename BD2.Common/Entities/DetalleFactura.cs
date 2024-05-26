using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class DetalleFactura
    {
        public int IdDetalleFactura { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioLista {  get; set; }
        public decimal PrecioVenta {  get; set; }
        public Factura Factura { get; set; }
        public Producto Producto { get; set; }
    }
}
