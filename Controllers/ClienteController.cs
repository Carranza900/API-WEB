using Integrador.Models;
using Microsoft.AspNetCore.Mvc;
using Integrador.Services;
using System.Collections.Generic;

namespace Integrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        // Inyectar ClienteService mediante el constructor
        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/cliente
        [HttpGet]
        public ActionResult<List<Cliente>> GetClientes()
        {
            var cliente = _clienteService.ObtenerClientes();
            return Ok(cliente);
        }

        // POST: api/cliente
        [HttpPost]
        public ActionResult<int> PostCliente(Cliente cliente)
        {
            var result = _clienteService.AgregarCliente(cliente);
            return Ok(result);
        }

        // PUT: api/cliente/5
        [HttpPut("{id}")]
        public IActionResult PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ID_Cliente)
            {
                return BadRequest();
            }
            var result = _clienteService.ActualizarCliente(cliente);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/cliente/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCliente(int id)
        {
            var result = _clienteService.EliminarCliente(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
