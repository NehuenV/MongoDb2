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

        [HttpGet(nameof(punto1))]
        public async Task<IActionResult> punto1(string from, string to)
        {
            try
            {
                var result = await _dataService.punto1(DateTime.Parse(from),DateTime.Parse(to));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(nameof(punto2))]
        public async Task<IActionResult> punto2(string from, string to)
        {
            try
            {
                var result = await _dataService.punto2(DateTime.Parse(from), DateTime.Parse(to));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(nameof(punto3))]
        public async Task<IActionResult> punto3(string from, string to)
        {
            try
            {
                var result = await _dataService.punto3(DateTime.Parse(from), DateTime.Parse(to));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(punto4))]
        public async Task<IActionResult> punto4(string from, string to)
        {
            try
            {
                var result = await _dataService.punto4(DateTime.Parse(from), DateTime.Parse(to));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(punto5))]
        public async Task<IActionResult> punto5()
        {
            try
            {
                var result = await _dataService.punto5();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(nameof(punto6))]
        public async Task<IActionResult> punto6()
        {
            try
            {
                var result = await _dataService.punto6();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(nameof(punto7))]
        public async Task<IActionResult> punto7()
        {
            try
            {
                var result = await _dataService.punto7();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(nameof(punto8))]
        public async Task<IActionResult> punto8()
        {
            try
            {
                var result = await _dataService.punto8();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
