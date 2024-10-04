using Integrador.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoIV.Services;
using System.Collections.Generic;

namespace ProyectoIV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/categoria
        [HttpGet]
        public ActionResult<List<Categoria>> GetCategorias()
        {
            var categorias = _categoriaService.ObtenerCategorias();
            return Ok(categorias);
        }

        // POST: api/categoria
        [HttpPost]
        public ActionResult<bool> PostCategoria([FromBody] Categoria categoria)
        {
            if (categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            {
                return BadRequest("La categoría no es válida.");
            }

            var result = _categoriaService.AgregarCategoria(categoria);
            if (!result)
            {
                return BadRequest("No se pudo agregar la categoría.");
            }

            return CreatedAtAction(nameof(GetCategorias), new { id = categoria.ID_Categoria }, categoria);
        }

        // PUT: api/categoria/5
        [HttpPut("{id}")]
        public IActionResult PutCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.ID_Categoria)
            {
                return BadRequest("El ID de la categoría no coincide.");
            }

            var result = _categoriaService.ActualizarCategoria(categoria);
            if (!result)
            {
                return NotFound("Categoría no encontrada.");
            }

            return NoContent();
        }

        // DELETE: api/categoria/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(int id)
        {
            var result = _categoriaService.EliminarCategoria(id);
            if (!result)
            {
                return NotFound("Categoría no encontrada.");
            }

            return NoContent();
        }
    }
}
