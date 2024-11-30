using Microsoft.AspNetCore.Mvc;
using Paletitas.Models;
using Paletitas.Services;

namespace Paletitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {

        private readonly VentaServices _ventasServices;

        public VentaController(VentaServices ventaServices)
        {
            _ventasServices = ventaServices;
        }

        [HttpPost("Registrar Venta")]
        public IActionResult Add([FromBody] VentasDtoSoli venta)
        {
            if (venta == null)
            {
                return BadRequest("Datos no encontrados");
            }

            var resultado = _ventasServices.Add(venta);

            if (resultado == null)
            {
                return BadRequest(new { message = "No se pudo realizar la venta debido a existencias insuficientes." });
            }

            return Ok("Venta registrada exitosamente.");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var venta = _ventasServices.GetAll();
            if (venta == null) { return NotFound(); }
            return Ok(venta);
        }


        [HttpGet ("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var venta = _ventasServices.GetById(id);

                if (venta == null)
                {
                    return NotFound(new
                    {
                        message = $"No se encontró una venta con el ID {id}."
                    });
                }

                return Ok(venta);
            }
          
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error en el controlador: {ex.Message}");
                return StatusCode(500, new { mensaje = $"Error al obtener la venta: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVenta(int id, [FromBody] VentasDtoSoli venta)
        {
            if (venta == null || id<=0)
            {
                return BadRequest("Datos inválidos, intente de nuevo por favor .");
            }
            var result = _ventasServices.UpdateVenta(id, venta);

            if (result==null)
            {
              return NotFound("Venta no encontrada." );
            }
            
            return Ok();

        }

    }

 }
    





