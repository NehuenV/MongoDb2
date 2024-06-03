using BD2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB2.Service.Interface
{
    public interface IDataService
    {
        Task<bool> AgregarData(int cantidad);
        Task<List<Factura>> ConsultarData();
        Task<dynamic> ConsultarAgrupadoEntreFechas();
    }
}
