using Integrador.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoIV.Services;
using System.Collections.Generic;

namespace ProyectoIV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuario
        [HttpGet]
        public ActionResult<List<Usuarios>> GetUsuarios()
        {
            var usuarios = _usuarioService.ObtenerUsuarios();
            return Ok(usuarios);
        }

        // POST: api/usuario
        [HttpPost]
        public ActionResult<bool> PostUsuario(Usuarios usuario)
        {
            var result = _usuarioService.AgregarUsuario(usuario);
            return Ok(result);
        }

        // PUT: api/usuario/5
        [HttpPut("{id}")]
        public IActionResult PutUsuario(int id, Usuarios usuario)
        {
            if (id != usuario.ID_Usuario)
            {
                return BadRequest();
            }

            var result = _usuarioService.ActualizarUsuario(usuario);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/usuario/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var result = _usuarioService.EliminarUsuario(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
