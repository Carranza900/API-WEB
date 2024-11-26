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
    public class ClienteController : ControllerBase
    {
        private readonly ClienteServices _clienteServices;

        public ClienteController(ClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
        }

        // GET: api/Cliente
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> Get()
        {
            try
            {
                var clientes = _clienteServices.ObtenerClientes();
                return Ok(clientes); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los clientes: {ex.Message}"); 
            }
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public ActionResult<Cliente> Get(int id)
        {
            try
            {
                var cliente = _clienteServices.ObtenerClientes().FirstOrDefault(c => c.ID_Cliente == id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return Ok(cliente); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el cliente: {ex.Message}");
            }
        }

        // POST: api/Cliente
        [HttpPost]
        public ActionResult Create([FromBody] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _clienteServices.AgregarCliente(cliente.Nombre, cliente.Apellido, cliente.Telefono);
                    return CreatedAtAction(nameof(Get), new { id = cliente.ID_Cliente }, cliente); 
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el cliente: {ex.Message}");
            }
        }

        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public ActionResult Edit(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (id != cliente.ID_Cliente)
                {
                    return BadRequest(); 
                }

                if (ModelState.IsValid)
                {
                    _clienteServices.ActualizarCliente(cliente.ID_Cliente, cliente.Nombre, cliente.Apellido, cliente.Telefono);
                    return NoContent(); 
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el cliente: {ex.Message}");
            }
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _clienteServices.EliminarCliente(id);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el cliente: {ex.Message}");
            }
        }
    }
}
