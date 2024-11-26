using Microsoft.AspNetCore.Mvc;
using Paletitas.Models;
using Paletitas.Services;
using System;
using System.Collections.Generic;

namespace Paletitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase 
    {
        private readonly ProveedorServices _proveedorServices;

        public ProveedorController(ProveedorServices proveedorServices)
        {
            _proveedorServices = proveedorServices;
        }

        // GET: api/Proveedor
        [HttpGet]
        public ActionResult<IEnumerable<Proveedor>> Get()
        {
            try
            {
                var proveedores = _proveedorServices.ObtenerProveedores();
                return Ok(proveedores); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los proveedores: {ex.Message}"); 
            }
        }

        // GET: api/Proveedor/5
        [HttpGet("{id}")]
        public ActionResult<Proveedor> Get(int id)
        {
            try
            {
                var proveedor = _proveedorServices.ObtenerProveedores().Find(p => p.ID_Proveedor == id);
                if (proveedor == null)
                {
                    return NotFound();
                }
                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el proveedor: {ex.Message}");
            }
        }

        // POST: api/Proveedor
        [HttpPost]
        public ActionResult Create(Proveedor proveedor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _proveedorServices.AgregarProveedor(proveedor);
                    return CreatedAtAction(nameof(Get), new { id = proveedor.ID_Proveedor }, proveedor); 
                }
                return BadRequest(ModelState); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el proveedor: {ex.Message}");
            }
        }

        // PUT: api/Proveedor/5
        [HttpPut("{id}")]
        public ActionResult Edit(int id, Proveedor proveedor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _proveedorServices.ActualizarProveedor(proveedor);
                    return NoContent(); 
                }
                return BadRequest(ModelState); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el proveedor: {ex.Message}");
            }
        }

        // DELETE: api/Proveedor/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _proveedorServices.EliminarProveedor(id);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el proveedor: {ex.Message}");
            }
        }
    }
}
