using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Integrador.Models;
using ProyectoIV.DataAccess; 

namespace Integrador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDAL _usuarioDAL;

        public AuthController(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL; // Inyección 
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel login)
        {
            // Valida credenciales utilizando la base de datos
            var usuario = _usuarioDAL.ValidarUsuario(login.Usuario, login.Clave);
            if (usuario != null)
            {
                var token = GenerarToken(usuario.Usuario);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        // Método para generar el token JWT
        private string GenerarToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("S3gur1dad#2024!@#SecretSuperSegura12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "AuthService", // Emisor del token
                audience: "MyApi", // Audiencia del token
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
