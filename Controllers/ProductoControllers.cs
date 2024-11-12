using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SISWIN.Services;
using SISWIN.Models;
using System.Collections.Generic;

namespace SISWIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _productoService = new ProductoService(connectionString);
        }

        // GET: api/Producto
        [HttpGet]
        public ActionResult<IEnumerable<Producto>> Get()
        {
            var productos = _productoService.ObtenerProductos();
            return Ok(productos);
        }

        // GET: api/Producto/5
        [HttpGet("{id}")]
        public ActionResult<Producto> Get(int id)
        {
            var productos = _productoService.ObtenerProductos();
            var producto = productos.Find(p => p.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public ActionResult<Producto> Post([FromBody] ProductoRequest productoRequest)
        {
            // Validaciones adicionales
            if (productoRequest == null)
            {
                return BadRequest("La solicitud no puede ser nula.");
            }

            if (string.IsNullOrEmpty(productoRequest.Nombre) || string.IsNullOrEmpty(productoRequest.Descripcion))
            {
                return BadRequest("El nombre y la descripción del producto son obligatorios.");
            }

            // Validar detalles
            if (productoRequest.Detalles == null || productoRequest.Detalles.Count == 0)
            {
                return BadRequest("El producto debe tener detalles.");
            }

            // Validar características
            if (productoRequest.Caracteristicas == null || productoRequest.Caracteristicas.Count == 0)
            {
                return BadRequest("El producto debe tener al menos una característica.");
            }

            try
            {
                _productoService.InsertarProductoConDetallesYCaracteristicas(
                    productoRequest.IdCategoria,
                    productoRequest.Nombre,
                    productoRequest.Descripcion,
                    productoRequest.Estado,
                    productoRequest.Detalles,
                    productoRequest.Caracteristicas
                );

                return CreatedAtAction(nameof(Get), new { id = productoRequest.IdCategoria }, productoRequest);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error al insertar producto: {ex.Message}");
            }
        }

    }
}
