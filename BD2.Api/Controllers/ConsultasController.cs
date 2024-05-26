using BD2.Common.Entities;
using DB2.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BD2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly IDataService _dataService;
        public ConsultasController( IDataService dataService) 
        {
            _dataService = dataService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Factura>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Factura>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var resultados = await _dataService.ConsultarData();
                return Ok(resultados);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
    }
}
