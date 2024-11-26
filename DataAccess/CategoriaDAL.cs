using Paletitas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Paletitas.DataAccess
{
    public class CategoriaDAL
    {
        private readonly string connectionString;

        public CategoriaDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Método para listar categorías
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ListarCategorias", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorias.Add(new Categoria
                        {
                            ID_Categoria = Convert.ToInt32(reader["ID_Categoria"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Estado = reader["Estado"].ToString()
                        });
                    }
                }
            }

            return categorias;
        }

        // Método para insertar una nueva categoría
        public void InsertarCategoria(string nombre, string descripcion, bool estado)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertarCategoria", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Estado", estado);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Método para actualizar una categoría existente
        public void ActualizarCategoria(int idCategoria, string nombre, string descripcion, bool estado)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ActualizarCategoria", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_Categoria", idCategoria);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Estado", estado);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Método para eliminar una categoría
        public void EliminarCategoria(int idCategoria)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("EliminarCategoria", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_Categoria", idCategoria);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
