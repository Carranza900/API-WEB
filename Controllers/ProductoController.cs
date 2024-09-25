using Integrador.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoIV.Services;
using System.Collections.Generic;

namespace ProyectoIV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService = new ProductoService();

        // GET: api/producto
        [HttpGet]
        public ActionResult<List<Producto>> GetProductos()
        {
            var productos = _productoService.ObtenerProductos();
            return Ok(productos);
        }

        // GET: api/producto/5
        [HttpGet("{id}")]
        public ActionResult<Producto> GetProducto(int id)
        {
            var producto = _productoService.ObtenerProductos().Find(p => p.ID_Producto == id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // POST: api/producto
        [HttpPost]
        public ActionResult<int> PostProducto(Producto producto)
        {
            var idProducto = _productoService.AgregarProducto(producto);
            return CreatedAtAction(nameof(GetProducto), new { id = idProducto }, producto);
        }

        // PUT: api/producto/5
        [HttpPut("{id}")]
        public IActionResult PutProducto(int id, Producto producto)
        {
            if (id != producto.ID_Producto)
            {
                return BadRequest();
            }

            var result = _productoService.ActualizarProducto(producto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/producto/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProducto(int id)
        {
            var result = _productoService.EliminarProducto(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
