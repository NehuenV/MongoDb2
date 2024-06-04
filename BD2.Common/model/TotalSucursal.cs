using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2.Common.model
{
    public class TotalSucursal<T>
    {
        public TotalSucursal( List<T> listaVentasPorSucursal, decimal totalSucursales) 
        {
            ListaVentasPorSucursal = listaVentasPorSucursal;
            TotalSucursales = totalSucursales;
        }

        public List<T> ListaVentasPorSucursal { get; set; }
        public decimal TotalSucursales { get; set; }
    }
}
