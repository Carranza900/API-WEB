using Microsoft.AspNetCore.Mvc;
using Paletitas.Models;
using Paletitas.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paletitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaServices _categoriaServices;

        public CategoriaController(CategoriaServices categoriaServices)
        {
            _categoriaServices = categoriaServices;
        }

        // GET: api/Categoria
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _categoriaServices.ObtenerCategorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las categorías: {ex.Message}");
            }
        }

        // GET: api/Categoria/5
        [HttpGet("{id}")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categorias = _categoriaServices.ObtenerCategorias();
                var categoria = categorias.FirstOrDefault(c => c.ID_Categoria == id);
                if (categoria == null)
                {
                    return NotFound();
                }
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la categoría: {ex.Message}");
            }
        }

        // POST: api/Categoria
        [HttpPost]
        public ActionResult Create([FromBody] Categoria categoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool estadoBool = categoria.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase);
                    _categoriaServices.AgregarCategoria(categoria.Nombre, categoria.Descripcion, estadoBool);
                    return CreatedAtAction(nameof(Get), new { id = categoria.ID_Categoria }, categoria);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la categoría: {ex.Message}");
            }
        }

        // PUT: api/Categoria/5
        [HttpPut("{id}")]
        public ActionResult Edit(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.ID_Categoria)
                {
                    return BadRequest("ID no coincide con el de la categoría.");
                }

                if (ModelState.IsValid)
                {
                    bool estadoBool = categoria.Estado.Equals("Activo", StringComparison.OrdinalIgnoreCase);
                    _categoriaServices.ActualizarCategoria(id, categoria.Nombre, categoria.Descripcion, estadoBool);
                    return NoContent();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la categoría: {ex.Message}");
            }
        }

        // DELETE: api/Categoria/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _categoriaServices.EliminarCategoria(id);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la categoría: {ex.Message}");
            }
        }
    }
}
