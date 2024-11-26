using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SISWIN.Services;
using SISWIN.Models;
using System.Collections.Generic;

namespace SISWIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _compraService;

        public CompraController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _compraService = new CompraService(connectionString);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Compra compra)
        {
            if (compra == null)
            {
                return BadRequest("Datos no encontrados");
            }
            _compraService.Add(compra);
            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Compra>> GetAll()
        {
            var compra = _compraService.GetAll();
            if (compra == null) { return NotFound(); }
            return Ok(compra);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var factura = _compraService.GetById(id);
            if (factura == null) { return NotFound(); }
            return Ok(factura);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateCompra(int id, [FromBody] Compra compra)
        {
            if (compra == null || compra.ID_Compra != id)
            {
                return BadRequest("Datos inválidos, intente de nuevo por favor .");
            }

            try
            {
                var result = _compraService.UpdateCompra(compra);

                if (result.Contains("Error"))
                {
                    return StatusCode(500, new { mensaje = result });
                }

                return Ok(new { mensaje = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al actualizar la compra: {ex.Message}" });
            }
        }
    }
}
