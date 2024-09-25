using Integrador.Models;
using ProyectoIV.DataAccess;
using System.Collections.Generic;

namespace ProyectoIV.Services
{
    public class UsuarioService
    {
        private readonly UsuarioDAL _usuarioDAL;

        public UsuarioService(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        public List<Usuarios> ObtenerUsuarios()
        {
            return _usuarioDAL.ListarUsuarios();
        }

        public bool AgregarUsuario(Usuarios usuario)
        {
            return _usuarioDAL.InsertarUsuario(usuario);
        }

        public bool ActualizarUsuario(Usuarios usuario)
        {
            return _usuarioDAL.ActualizarUsuario(usuario);
        }

        public bool EliminarUsuario(int idUsuario)
        {
            return _usuarioDAL.EliminarUsuario(idUsuario);
        }
    }
}
