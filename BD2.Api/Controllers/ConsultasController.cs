using BD2.Common.Entities;
using BD2.Common.model;
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
        [HttpGet(nameof(GetData))]
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
        [HttpPost(nameof(GenerateData))]
        public async Task<IActionResult> GenerateData([FromBody] Request request)
        {
            try
            {
                var result = await _dataService.AgregarData(request.cantidad);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(ConsultarAgrupadoEntreFechas))]
        public async Task<IActionResult> ConsultarAgrupadoEntreFechas()
        {
            try
            {
                var result = await _dataService.ConsultarAgrupadoEntreFechas();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
