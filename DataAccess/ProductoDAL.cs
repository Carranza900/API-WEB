using Paletitas.Models;
using System.Data.SqlClient;
using System.Data;

namespace Paletitas.DataAccess
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

                                // Ejecuto el procedimiento almacenado
                                command.ExecuteNonQuery();
                            }

                            // Confirmamos la transacción
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            // Deshacemos la transacción si ocurre un error
                            transaction.Rollback();
                            throw; 
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
                connection.Open();
                using (var command = new SqlCommand("ListarProductos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new Producto
                            {
                                IdProducto = reader.GetInt32(reader.GetOrdinal("ID_Producto")),
                                Nombre = reader.GetString(reader.GetOrdinal("Producto")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                EstadoProducto = reader.GetString(reader.GetOrdinal("EstadoProducto")),
                                Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
                                Almacen = reader.IsDBNull(reader.GetOrdinal("Almacen")) ? null : reader.GetString(reader.GetOrdinal("Almacen")),
                                Ubicacion = reader.IsDBNull(reader.GetOrdinal("Ubicacion")) ? null : reader.GetString(reader.GetOrdinal("Ubicacion")),
                                Existencia = reader.IsDBNull(reader.GetOrdinal("Existencia")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Existencia")),
                                PrecioCompra = reader.IsDBNull(reader.GetOrdinal("Precio_Compra")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio_Compra")),
                                PrecioVenta = reader.IsDBNull(reader.GetOrdinal("Precio_Venta")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Precio_Venta"))
                            };
                            productos.Add(producto);
                        }
                    }
                }
            }

            return productos;
        }


    }
}
