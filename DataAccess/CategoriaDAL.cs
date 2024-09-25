using Integrador.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace ProyectoIV.DataAccess
{
    public class CategoriaDAL
    {
        private string cadenaConexion = "Data Source=DESKTOP-DEL1U7K;Initial Catalog=Paletitas;Integrated Security=True;TrustServerCertificate=True;";


        // Método para listar categorías
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ListarCategorias", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria
                        {
                            ID_Categoria = reader.GetInt32(reader.GetOrdinal("ID_Categoria")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader["Estado"].ToString() == "Activo" 
                        };
                        listaCategorias.Add(categoria);
                    }
                }
            }

            return listaCategorias;
        }

        // Método para insertar categoría
        public bool InsertarCategoria(Categoria categoria)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertarCategoria", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                command.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                command.Parameters.AddWithValue("@Estado", categoria.Estado);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        // Método para actualizar categoría
        public bool ActualizarCategoria(Categoria categoria)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ActualizarCategoria", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Categoria", categoria.ID_Categoria);
                command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                command.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                command.Parameters.AddWithValue("@Estado", categoria.Estado);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        // Método para eliminar categoría
        public bool EliminarCategoria(int idCategoria)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("EliminarCategoria", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Categoria", idCategoria);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }
    }
}
