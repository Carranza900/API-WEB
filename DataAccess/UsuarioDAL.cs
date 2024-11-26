using System.Data;
using System.Data.SqlClient;
using Paletitas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paletitas.Services;

namespace Paletitas.DataAccess
{
    public class UsuarioDAL
    {
        private readonly string _connectionString;

        public UsuarioDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método asincrónico para obtener usuarios
        public async Task<List<UsuarioDTO>> ObtenerUsuariosAsync()
        {
            List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("obtener_usuarios", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new UsuarioDTO
                            {
                                ID_Usuario = reader.GetInt32(reader.GetOrdinal("ID_Usuario")),
                                Usuario = reader.GetString(reader.GetOrdinal("UsuarioName")),
                                Rol = reader.GetString(reader.GetOrdinal("Rol")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")) == "Activo" ? "Activo" : "Inactivo"
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

        // Método asincrónico para insertar usuario
        public async Task InsertarUsuarioAsync(string usuarioName, string usuarioClave, int idRol, bool estado)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("insertar_usuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Encriptar la contraseña antes de guardarla
                    string hashedPassword = Paletitas.Services.PasswordHelper.EncryptPassword(usuarioClave);
                    Console.WriteLine(hashedPassword);

                    command.Parameters.AddWithValue("@UsuarioName", usuarioName);
                    command.Parameters.AddWithValue("@UsuarioClave", hashedPassword);
                    command.Parameters.AddWithValue("@Id_Rol", idRol);
                    command.Parameters.AddWithValue("@Estado", estado);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Método  para actualizar usuario
        public async Task ActualizarUsuarioAsync(int idUsuario, string usuarioName, string usuarioClave, int idRol, bool estado)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("actualizar_usuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Encriptar la contraseña antes de actualizarla
                    string hashedPassword = Paletitas.Services.PasswordHelper.EncryptPassword(usuarioClave);

                    command.Parameters.AddWithValue("@ID_Usuario", idUsuario);
                    command.Parameters.AddWithValue("@UsuarioName", usuarioName);
                    command.Parameters.AddWithValue("@UsuarioClave", hashedPassword);
                    command.Parameters.AddWithValue("@Id_Rol", idRol);
                    command.Parameters.AddWithValue("@Estado", estado);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar usuario
        public async Task EliminarUsuarioAsync(int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("eliminar_usuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Usuario", idUsuario);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Modificar el método en UsuarioDAL
        public async Task<UsuarioDTO> AutenticarUsuarioAsync(UserLoginModel loginModel)
        {
            UsuarioDTO usuario = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();


                using (SqlCommand command = new SqlCommand("AutenticarUsuario2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Usuario", loginModel.Usuario);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Obtiene la contraseña encriptada de la base de datos
                            string hashedPassword = reader.GetString(reader.GetOrdinal("UsuarioClave"));

                            // Verifica si la contraseña proporcionada coincide con la almacenada
                            if (PasswordHelper.VerifyPassword(loginModel.Clave, hashedPassword))
                            {
                                usuario = new UsuarioDTO
                                {
                                    ID_Usuario = reader.GetInt32(reader.GetOrdinal("ID_Usuario")),
                                    Usuario = reader.GetString(reader.GetOrdinal("UsuarioName")),
                                    Rol = reader.GetString(reader.GetOrdinal("Rol")),
                                    Estado = reader.GetString(reader.GetOrdinal("Estado"))
                                };
                            }
                        }
                    }
                }
            }

            return usuario;
        }




    }
}
