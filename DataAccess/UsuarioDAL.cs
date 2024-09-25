using Integrador.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ProyectoIV.DataAccess
{
    public class UsuarioDAL
    {
        private string cadenaConexion = "Data Source=DESKTOP-DEL1U7K;Initial Catalog=Paletitas;Integrated Security=True;TrustServerCertificate=True;";


        // Método para validar usuario
        public Usuarios ValidarUsuario(string usuario, string clave)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("AutenticarUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Clave", clave); 

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuarios
                        {
                            ID_Usuario = reader.GetInt32(reader.GetOrdinal("ID_Usuario")),
                            Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                            Rol = reader.GetString(reader.GetOrdinal("Rol")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
                        };
                    }
                }
            }
            return null; 
        }




        // Método para listar usuarios
        public List<Usuarios> ListarUsuarios()
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ListarUsuarios", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuarios usuario = new Usuarios
                        {
                            ID_Usuario = reader.GetInt32(reader.GetOrdinal("ID_Usuario")),
                            Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                            Rol = reader.GetString(reader.GetOrdinal("Rol")),
                            Estado = reader["Estado"].ToString() == "Activo" 
                        };
                        listaUsuarios.Add(usuario);
                    }
                }
            }

            return listaUsuarios;
        }

        // Método para insertar usuario
        public bool InsertarUsuario(Usuarios usuario)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertarUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                command.Parameters.AddWithValue("@Clave", usuario.Clave);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@Estado", usuario.Estado ? "Activo" : "Inactivo");

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        // Método para actualizar usuario
        public bool ActualizarUsuario(Usuarios usuario)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ActualizarUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Usuario", usuario.ID_Usuario);
                command.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                command.Parameters.AddWithValue("@Clave", usuario.Clave);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@Estado", usuario.Estado ? "Activo" : "Inactivo");

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        // Método para eliminar usuario
        public bool EliminarUsuario(int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("EliminarUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Usuario", idUsuario);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }
    }
}
