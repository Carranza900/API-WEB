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
    [Authorize]  // Solo accesible si se proporciona un token JWT válido
    public ActionResult<List<Usuarios>> GetUsuarios()
    {
        var usuarios = _usuarioService.ObtenerUsuarios();
        return Ok(usuarios);
    }

    // POST: api/usuario
    [HttpPost]
    [AllowAnonymous]  // Permitido sin autenticación, para registrar nuevos usuarios
    public ActionResult<bool> PostUsuario(Usuarios usuario)
    {
        var result = _usuarioService.AgregarUsuario(usuario);
        return Ok(result);
    }

    // PUT: api/usuario/5
    [HttpPut("{id}")]
    [Authorize]  // Protegido con JWT
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
