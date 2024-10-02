using Integrador.Models;
using Microsoft.AspNetCore.Mvc;
using Integrador.Services;
using System.Collections.Generic;
//using ProyectoIV.Services;

namespace Integrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController: ControllerBase
    {
        private readonly ClienteService _ClienteService = new ClienteService();

        // GET: api/categoria
        [HttpGet]
        public ActionResult<List<Cliente>> GetCategorias()
        {
            var cliente = _ClienteService.ObtenerClientes();
            return Ok(cliente);
        }

        // POST: api/categoria
        [HttpPost]
        public ActionResult<int> PostCliente(Cliente cliente)
        {
            var result = _ClienteService.AgregarCliente(cliente);
            return Ok(result);
        }

        // PUT: api/categoria/5
        [HttpPut("{id}")]
        public IActionResult PutCliente  (int id, Cliente cliente)
        {
            if (id != cliente.ID_Cliente)
            {
                return BadRequest();
            }
            var result = _ClienteService.ActualizarCliente(cliente);
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
            var result = _ClienteService.EliminarCliente(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
