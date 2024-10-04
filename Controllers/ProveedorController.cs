using Integrador.Models;
using Integrador.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Integrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly ProveedorService _proveedorService;

        public ProveedorController(ProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        // GET: api/proveedor
        [HttpGet]
        public ActionResult<List<Proveedor>> GetProveedores()
        {
            var proveedores = _proveedorService.ObtenerProveedores();
            return Ok(proveedores);
        }

        // GET: api/proveedor/5
        [HttpGet("{id}")]
        public ActionResult<Proveedor> GetProveedor(int id)
        {
            var proveedor = _proveedorService.ObtenerProveedores().Find(p => p.ID_Proveedor == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return Ok(proveedor);
        }

        // POST: api/proveedor
        [HttpPost]
        public ActionResult<int> PostProveedor(Proveedor proveedor)
        {
            var idProveedor = _proveedorService.AgregarProveedor(proveedor);
            return CreatedAtAction(nameof(GetProveedor), new { id = idProveedor }, proveedor);
        }

        // PUT: api/proveedor/5
        [HttpPut("{id}")]
        public IActionResult PutProveedor(int id, Proveedor proveedor)
        {
            if (id != proveedor.ID_Proveedor)
            {
                return BadRequest();
            }

            var result = _proveedorService.ActualizarProveedor(proveedor);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/proveedor/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProveedor(int id)
        {
            var result = _proveedorService.EliminarProveedor(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
