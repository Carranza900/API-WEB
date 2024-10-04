using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIV.Services;
using System.Collections.Generic;
using Integrador.Models;
using System.Linq; 

namespace ProyectoIV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/producto
        [HttpGet]
        public ActionResult<List<ProductoDto>> GetProductos()
        {
            var productos = _productoService.ObtenerProductos();

            // Convertir productos a ProductoDto
            var productosDto = productos.Select(p => new ProductoDto
            {
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                EstadoTexto = p.EstadoTexto,
                NombreCategoria = p.NombreCategoria
            }).ToList();

            return Ok(productosDto);
        }


        // GET: api/producto/5
        [HttpGet("{id}")]
        public ActionResult<ProductoDto> GetProducto(int id)
        {
            var producto = _productoService.ObtenerProductos().FirstOrDefault(p => p.ID_Producto == id);

            if (producto == null)
            {
                return NotFound("Producto no encontrado.");
            }

            // Convertir el producto a ProductoDto
            var productoDto = new ProductoDto
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                EstadoTexto = producto.EstadoTexto,
                NombreCategoria = producto.NombreCategoria
            };

            return Ok(productoDto);
        }

        // POST: api/producto
        [HttpPost]
        public ActionResult<int> PostProducto([FromBody] ProductoDto productoDto)
        {
            if (productoDto == null)
            {
                return BadRequest("Datos del producto inválidos.");
            }

            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                EstadoTexto = productoDto.EstadoTexto,
                NombreCategoria = productoDto.NombreCategoria,
                Id_Categoria = productoDto.Id_Categoria 
            };

            if (!_productoService.CategoriaExiste(producto.Id_Categoria))
            {
                return BadRequest("La categoría especificada no existe.");
            }

            var idProducto = _productoService.AgregarProducto(producto);
            return CreatedAtAction(nameof(GetProducto), new { id = idProducto }, new { ID_Producto = idProducto });
        }

        // PUT: api/producto/5
        [HttpPut("{id}")]
        public IActionResult PutProducto(int id, [FromBody] ProductoDto productoDto)
        {
            if (productoDto == null)
            {
                return BadRequest("Datos del producto inválidos.");
            }

            var productoExistente = _productoService.ObtenerProductos().Find(p => p.ID_Producto == id);
            if (productoExistente == null)
            {
                return NotFound("Producto no encontrado.");
            }

            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Descripcion = productoDto.Descripcion;
            productoExistente.EstadoTexto = productoDto.EstadoTexto;
            productoExistente.Id_Categoria = productoDto.Id_Categoria; 
            productoExistente.Estado = productoDto.Estado;

            var result = _productoService.ActualizarProducto(productoExistente);
            if (!result)
            {
                return StatusCode(500, "Error al actualizar el producto.");
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
                return NotFound("Producto no encontrado.");
            }

            return NoContent();
        }
    }
}
