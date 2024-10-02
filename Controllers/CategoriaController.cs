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

        // Inyección de dependencias a través del constructor
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
        public ActionResult<bool> PostCategoria(Categoria categoria)
        {
            var result = _categoriaService.AgregarCategoria(categoria);
            return Ok(result);
        }

        // PUT: api/categoria/5
        [HttpPut("{id}")]
        public IActionResult PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.ID_Categoria)
            {
                return BadRequest();
            }

            var result = _categoriaService.ActualizarCategoria(categoria);
            if (!result)
            {
                return NotFound();
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
                return NotFound();
            }

            return NoContent();
        }
    }
}
