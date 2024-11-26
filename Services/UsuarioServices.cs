using Paletitas.Models;
using System.Data.SqlClient;
using System.Data;
using Paletitas.Services;

public class UsuarioServices
{
    private readonly string _connectionString;

    public UsuarioServices(string connectionString)
    {
        _connectionString = connectionString;
    }

    // **Método para obtener la lista de usuarios**
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

    public async Task InsertarUsuarioAsync(string usuarioName, string usuarioClave, int idRol, bool estado)
    {
        if (string.IsNullOrWhiteSpace(usuarioName))
        {
            throw new ArgumentException("El nombre de usuario no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(usuarioClave) || usuarioClave.Length < 8)
        {
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("insertar_usuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Encriptar la contraseña antes de guardarla
                    string hashedPassword = Paletitas.Services.PasswordHelper.EncryptPassword(usuarioClave);

                    // Agregar los parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@UsuarioName", usuarioName);
                    command.Parameters.AddWithValue("@UsuarioClave", hashedPassword);
                    command.Parameters.AddWithValue("@Id_Rol", idRol);
                    command.Parameters.AddWithValue("@Estado", estado);

                    // Ejecutar el comando
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (SqlException ex)
        {
            throw new Exception("Error al insertar el usuario en la base de datos.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Ocurrió un error inesperado al insertar el usuario.", ex);
        }
    }

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
                command.Parameters.AddWithValue("@UsuarioClave", hashedPassword); // Actualiza la contraseña encriptada
                command.Parameters.AddWithValue("@Id_Rol", idRol);
                command.Parameters.AddWithValue("@Estado", estado);

                await command.ExecuteNonQueryAsync();
            }
        }
    }


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

    // Método de autenticación de usuario
    public async Task<UsuarioDTO> AutenticarUsuarioAsync(UserLoginModel loginModel)
    {
        UsuarioDTO usuario = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand("AutenticarUsuario", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Usuario", loginModel.Usuario);
                command.Parameters.AddWithValue("@Clave", loginModel.Clave);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string hashedPassword = reader.GetString(reader.GetOrdinal("UsuarioClave"));
                        if (PasswordHelper.VerifyPassword(loginModel.Clave, hashedPassword))
                        {
                            usuario = new UsuarioDTO
                            {
                                ID_Usuario = reader.GetInt32(reader.GetOrdinal("ID_Usuario")),
                                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
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
