using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SISWIN.Models;

namespace SISWIN.DataAccess
{
    public class ProductoDAL
    {
        private readonly string _connectionString;

        public ProductoDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para insertar un producto con detalles y características
        public void InsertarProducto(int idCategoria, string nombre, string descripcion, bool estado, DataTable detalles, DataTable caracteristicas)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand("InsertarProducto", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Parámetros del producto
                            command.Parameters.AddWithValue("@Id_Categoria", idCategoria);
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Descripcion", descripcion);
                            command.Parameters.AddWithValue("@Estado", estado);

                            // Parámetro para los detalles (TVP)
                            var tvpDetalles = new SqlParameter("@Detalles", SqlDbType.Structured)
                            {
                                TypeName = "DetalleProductoAlmacenType",
                                Value = detalles
                            };
                            command.Parameters.Add(tvpDetalles);

                            // Parámetro para las características (TVP)
                            var tvpCaracteristicas = new SqlParameter("@Caracteristicas", SqlDbType.Structured)
                            {
                                TypeName = "CaracteristicasType",
                                Value = caracteristicas
                            };
                            command.Parameters.Add(tvpCaracteristicas);

                            // Ejecutar el procedimiento almacenado
                            command.ExecuteNonQuery();
                        }

                        // Confirmar la transacción
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Deshacetransacción si ocurre un error
                        transaction.Rollback();
                        throw; // Propagar el error al controlador
                    }
                }
            }
        }

        // Método para listar productos
        public List<Producto> ListarProductos()
        {
            var productos = new List<Producto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("ListarProductos", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idProducto = reader.IsDBNull(reader.GetOrdinal("ID_Producto")) ? 0 : (int)reader["ID_Producto"];
                        var producto = productos.FirstOrDefault(p => p.IdProducto == idProducto);

                        if (producto == null)
                        {
                            producto = new Producto
                            {
                                IdProducto = idProducto,
                                Nombre = reader["Producto"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                EstadoProducto = reader["EstadoProducto"].ToString(),
                                Categoria = reader["Categoria"].ToString(),
                                Almacen = reader["Almacen"].ToString(),
                                Ubicacion = reader["Ubicacion"].ToString(),
                                Existencia = reader.IsDBNull(reader.GetOrdinal("Existencia")) ? 0 : (int)reader["Existencia"],
                                PrecioCompra = reader.IsDBNull(reader.GetOrdinal("Precio_Compra")) ? 0 : (decimal)reader["Precio_Compra"],
                                PrecioVenta = reader.IsDBNull(reader.GetOrdinal("Precio_Venta")) ? 0 : (decimal)reader["Precio_Venta"]
                            };

                            productos.Add(producto);
                        }

                        // Agregamos característicos
                        if (!reader.IsDBNull(reader.GetOrdinal("Caracteristica")))
                        {
                            producto.Caracteristicas.Add(reader["Caracteristica"].ToString());
                        }
                    }
                }
            }

            return productos;
        }
    }
}
