using Integrador.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIV.Services;

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
    [Authorize]
    public ActionResult<List<UsuarioDto>> GetUsuarios()
    {
        var usuarios = _usuarioService.ObtenerUsuarios();

        // Convertir la lista de Usuarios a UsuarioDto
        var usuariosDto = usuarios.Select(u => new UsuarioDto
        {
            Usuario = u.Usuario,
            Clave = u.Clave,
            Rol = u.Rol,
            Estado = u.Estado ? "Activo" : "Inactivo"
        }).ToList();

        return Ok(usuariosDto);
    }

    // POST: api/usuario
    [HttpPost]
    [AllowAnonymous]  // sin autenticación, para registrar nuevos usuarios
    public ActionResult<bool> PostUsuario([FromBody] UsuarioDto usuarioDto)
    {
        // Convertir el estado de string a bool
        bool estadoBool = usuarioDto.Estado == "Activo";

        var usuario = new Usuarios
        {
            Usuario = usuarioDto.Usuario,
            Clave = usuarioDto.Clave,
            Rol = usuarioDto.Rol,
            Estado = estadoBool // Convertimos el estado a bool
        };

        var result = _usuarioService.AgregarUsuario(usuario);
        return Ok(result);
    }

    // PUT: api/usuario/5
    [HttpPut("{id}")]
    [Authorize]  // Protegido con JWT
    public IActionResult PutUsuario(int id, [FromBody] UsuarioDto usuarioDto)
    {
        if (id <= 0)
        {
            return BadRequest("ID de usuario inválido.");
        }

        // Convertir el estado de string a bool
        bool estadoBool = usuarioDto.Estado == "Activo";

        var usuario = new Usuarios
        {
            ID_Usuario = id,
            Usuario = usuarioDto.Usuario,
            Clave = usuarioDto.Clave,
            Rol = usuarioDto.Rol,
            Estado = estadoBool
        };

        var result = _usuarioService.ActualizarUsuario(usuario);
        if (!result)
        {
            return NotFound("Usuario no encontrado.");
        }

        return NoContent();
    }

    // DELETE: api/usuario/5
    [HttpDelete("{id}")]
    [Authorize]  // Protegido con JWT
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
