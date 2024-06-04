using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    [BsonIgnoreExtraElements]
    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdTicket { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal TotalVenta { get; set; }
        public FormasDePago FormasDePago { get; set; }
        public Empleado EmpleadoCaja { get; set; }
        public Empleado EmpleadoVenta { get; set; }
        public Empleado Encargado { get; set; }
        public Persona Cliente { get; set; }
        public Sucursal Sucursal { get; set; }
        public List<DetalleFactura> DetalleFactura { get; set; }
    }
}
