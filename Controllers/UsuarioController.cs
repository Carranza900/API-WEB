using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Paletitas.DataAccess;
using Paletitas.Models;
using System.Threading.Tasks;

namespace Paletitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioDAL _usuarioDAL;

        public UsuarioController(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _usuarioDAL.ObtenerUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al obtener los usuarios: {ex.Message}");
            }
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<IActionResult> InsertarUsuario([FromBody] Usuarios nuevoUsuario)
        {
            try
            {
                if (nuevoUsuario == null)
                {
                    return BadRequest("Los datos del usuario son requeridos.");
                }

                // Validaciones de campos obligatorios
                if (string.IsNullOrWhiteSpace(nuevoUsuario.UsuarioName) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.UsuarioClave) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.Rol) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.Estado))
                {
                    return BadRequest("Todos los campos son obligatorios.");
                }

                // Validación y conversión del Rol
                int idRol = nuevoUsuario.Rol.ToLower() switch
                {
                    "administrador" => 1,
                    "empleado" => 2,
                    _ => throw new ArgumentException("Rol no válido. Los roles permitidos son: Administrador o Empleado.")
                };

                // Validación y conversión del Estado
                bool estado = nuevoUsuario.Estado.ToLower() switch
                {
                    "activo" => true,
                    "inactivo" => false,
                    _ => throw new ArgumentException("Estado no válido. Los estados permitidos son: Activo o Inactivo.")
                };

                // Llama al método para insertar el usuario en la base de datos
                await _usuarioDAL.InsertarUsuarioAsync(
                    nuevoUsuario.UsuarioName,
                    nuevoUsuario.UsuarioClave,
                    idRol,
                    estado
                );

                return Ok("Usuario insertado correctamente.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocurrió un error al insertar el usuario: {ex.Message}");
            }
        }

        // PUT: api/Usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuarios usuarioActualizado)
        {
            try
            {
                if (usuarioActualizado == null)
                {
                    return BadRequest("Los datos del usuario son requeridos.");
                }

                // Validaciones de campos obligatorios
                if (string.IsNullOrWhiteSpace(usuarioActualizado.UsuarioName) ||
                    string.IsNullOrWhiteSpace(usuarioActualizado.UsuarioClave) ||
                    string.IsNullOrWhiteSpace(usuarioActualizado.Rol) ||
                    string.IsNullOrWhiteSpace(usuarioActualizado.Estado))
                {
                    return BadRequest("Todos los campos son obligatorios.");
                }

                // Validación y conversión del Rol
                int idRol = usuarioActualizado.Rol.ToLower() switch
                {
                    "administrador" => 1,
                    "empleado" => 2,
                    _ => throw new ArgumentException("Rol no válido. Los roles permitidos son: Administrador o Empleado.")
                };

                // Validación y conversión del Estado
                bool estado = usuarioActualizado.Estado.ToLower() switch
                {
                    "activo" => true,
                    "inactivo" => false,
                    _ => throw new ArgumentException("Estado no válido. Los estados permitidos son: Activo o Inactivo.")
                };

                // Llama al método para actualizar el usuario
                await _usuarioDAL.ActualizarUsuarioAsync(id, usuarioActualizado.UsuarioName, usuarioActualizado.UsuarioClave, idRol, estado);

                return Ok("Usuario actualizado correctamente.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocurrió un error al actualizar el usuario: {ex.Message}");
            }
        }

        // DELETE: api/Usuario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                // Llama al método para eliminar el usuario
                await _usuarioDAL.EliminarUsuarioAsync(id);
                return Ok("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ocurrió un error al eliminar el usuario: {ex.Message}");
            }
        }
    }
}
