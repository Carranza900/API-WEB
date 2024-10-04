using Integrador.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoIV.DataAccess
{
    public class ProductoDAL
    {
        private readonly string _cadenaConexion;

        public ProductoDAL(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("DefaultConnection");
        }

        // Método para listar productos
        public List<Producto> ListarProductos()
        {
            List<Producto> listaProductos = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("listar_productos", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto
                        {
                            ID_Producto = (int)reader["ID_Producto"],
                            Nombre = reader["NombreProducto"].ToString(),
                            Descripcion = reader["DescripcionProducto"].ToString(),
                            EstadoTexto = reader["EstadoProductoTexto"].ToString(),
                            NombreCategoria = reader["NombreCategoria"].ToString()
                        };
                        listaProductos.Add(producto);
                    }
                }
            }

            return listaProductos;
        }


        // Método para insertar producto
        public int InsertarProducto(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insertar_producto", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id_Categoria", producto.Id_Categoria);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                command.Parameters.AddWithValue("@Estado", producto.Estado);

                var idProducto = command.ExecuteScalar();
                return Convert.ToInt32(idProducto);
            }
        }


        // Método para actualizar producto
        public bool ActualizarProducto(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ActualizarProducto", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Producto", producto.ID_Producto);
                command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                command.Parameters.AddWithValue("@Estado", producto.Estado);
                command.Parameters.AddWithValue("@Id_Categoria", producto.Id_Categoria);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
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


        // Método para eliminar producto
        public bool EliminarProducto(int idProducto)
        {
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("EliminarProducto", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Producto", idProducto);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }
    }
}
