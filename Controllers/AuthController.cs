using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Paletitas.DataAccess;
using Paletitas.Models;
using Paletitas.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Paletitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDAL _usuarioDAL;
        private readonly JwtSettings _jwtSettings;

        public AuthController(UsuarioDAL usuarioDAL, IOptions<JwtSettings> jwtSettings)
        {
            _usuarioDAL = usuarioDAL;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Usuario) || string.IsNullOrWhiteSpace(loginModel.Clave))
            {
                return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
            }

            var usuario = await _usuarioDAL.AutenticarUsuarioAsync(loginModel);

            if (usuario == null)
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            // Generar el token
            var token = GenerarToken(usuario);

            return Ok(new
            {
                usuario.Usuario,
                usuario.Rol,
                Token = token
            });
        }

        private string GenerarToken(UsuarioDTO usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Rol", usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenLifetimeMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
