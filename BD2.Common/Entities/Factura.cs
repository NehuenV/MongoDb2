﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.Entities
{
    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdTicket { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal TotalVenta { get; set; }
        public FormasDePago FormasDePago { get; set; }
        public Persona EmpleadoCaja { get; set; }
        public Persona EmpleadoVenta { get; set; }
        public Persona Encargado { get; set; }
        public Persona Cliente { get; set; }
        public Sucursal Sucursal { get; set; }
        public DetalleFactura DetalleFactura { get; set; }
    }
}