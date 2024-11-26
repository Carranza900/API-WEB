using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SISWIN.Models;

namespace SISWIN.DataAccess
{
    public class CompraDAL
    {
        private readonly string _connectionString;

        public CompraDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Compra> GetAll()
        {
            var compras = new List<Compra>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Obtener todas las compras
                using (var command = new SqlCommand("SELECT * FROM Compras", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            compras.Add(new Compra
                            {
                                ID_Compra = (int)reader["ID_Compra"],
                                NombreProveedor = (string)reader["NombreProveedor"],
                                NombreUsuario = (string)reader["NombreUsuario"],
                                NombreProducto = (string)reader["NombreProducto"],
                                Num_Factura = reader["Num_Factura"].ToString(),
                                Cantidad = (int)reader["Cantidad"],
                                FechaCompra = (DateTime)reader["FechaCompra"],
                                SubTotal = (decimal)reader["Subtotal"],
                                IVA = (decimal)reader["IVA"],
                                Total = (decimal)reader["Total"],
                                DetalleCompra = new List<DetallesCompra>()
                            });
                        }
                    }
                }

                // Obtener todos los detalles de las compras
                using (var command = new SqlCommand("SELECT * FROM DetalleCompra", connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var detalle = new DetallesCompra
                        {
                            ID_Detalle_Compra = (int)reader["ID_Detalle_Compra"],
                            Id_Compra = (int)reader["Id_Compra"],
                            Id_DetalleProducto = (int)reader["Id_DetalleProducto"],
                            Precio_Compra = (decimal)reader["Precio_Compra"],
                            Cantidad = (int)reader["Cantidad"],
                            SubTotal = (decimal)reader["Subtotal"]
                        };

                        var compra = compras.FirstOrDefault(find => find.ID_Compra == detalle.Id_Compra);
                        if (compra != null)
                        {
                            compra.DetalleCompra.Add(detalle);
                        }
                    }
                }

                return compras;
            }
        }

        public Compra Add(Compra compra)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("sp_CrearCompra", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros principales
                        command.Parameters.AddWithValue("@NombreProveedor", compra.NombreProveedor);
                        command.Parameters.AddWithValue("@NombreUsuario", compra.NombreUsuario);
                        command.Parameters.AddWithValue("@NombreProducto", compra.NombreProducto);
                        command.Parameters.AddWithValue("@Num_Factura", compra.Num_Factura);
                        command.Parameters.AddWithValue("@Cantidad", compra.Cantidad);
                        command.Parameters.AddWithValue("@FechaCompra", compra.FechaCompra);
                        command.Parameters.AddWithValue("@Subtotal", compra.SubTotal);
                        command.Parameters.AddWithValue("@IVA", compra.IVA);
                        command.Parameters.AddWithValue("@Total", compra.Total);

                        // Crear DataTable para los detalles de compra
                        DataTable detalleTable = new DataTable();
                        detalleTable.Columns.Add("Id_DetalleProducto", typeof(int));
                        detalleTable.Columns.Add("Precio_Compra", typeof(decimal));
                        detalleTable.Columns.Add("Cantidad", typeof(int));
                        detalleTable.Columns.Add("Subtotal", typeof(decimal));

                        foreach (var detalle in compra.DetalleCompra)
                        {
                            detalleTable.Rows.Add(detalle.Id_DetalleProducto, detalle.Precio_Compra, detalle.Cantidad, detalle.SubTotal);
                        }

                        // Agregar parámetro para los detalles
                        SqlParameter detalleParameter = new SqlParameter("@Detalles", SqlDbType.Structured)
                        {
                            TypeName = "dbo.CompraDetallesTypes",
                            Value = detalleTable
                        };
                        command.Parameters.Add(detalleParameter);

                        command.ExecuteNonQuery();

                        return compra;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
        }



        public Compra GetById(int id)
        {
            var compra = new Compra();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta para obtener los datos de la compra
                using (var command = new SqlCommand("SELECT * FROM Compras WHERE ID_Compra = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            compra.ID_Compra = reader.GetInt32(0);
                            compra.NombreProveedor = reader.GetString(1);
                            compra.NombreUsuario = reader.GetString(2);
                            compra.NombreProducto = reader.GetString(3);
                            compra.Num_Factura = reader.GetString(4);
                            compra.Cantidad = reader.GetInt32(5);
                            compra.FechaCompra = reader.GetDateTime(6);
                            compra.SubTotal = reader.GetDecimal(7);
                            compra.IVA = reader.GetDecimal(8);
                            compra.Total = reader.GetDecimal(9);
                        }
                    }
                }

                // Consulta para obtener los detalles de la compra
                using (var command = new SqlCommand("SELECT * FROM DetalleCompra WHERE Id_Compra = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        compra.DetalleCompra = new List<DetallesCompra>();
                        while (reader.Read())
                        {
                            compra.DetalleCompra.Add(new DetallesCompra
                            {
                                ID_Detalle_Compra = reader.GetInt32(0),
                                Id_Compra = reader.GetInt32(1),
                                Id_DetalleProducto = reader.GetInt32(2),
                                Precio_Compra = reader.GetDecimal(3),
                                Cantidad = reader.GetInt32(4),
                                SubTotal = reader.GetDecimal(5)
                            });
                        }
                    }
                }
            }

            return compra;
        }

        public string UpdateCompra(Compra compra)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("ActualizarCompra", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros                       
                        command.Parameters.AddWithValue("@NombreProveedor", compra.NombreProveedor);
                        command.Parameters.AddWithValue("@NombreUsuario", compra.NombreUsuario);
                        command.Parameters.AddWithValue("@NombreProducto", compra.NombreProducto); 
                        command.Parameters.AddWithValue("@Cantidad", compra.Cantidad);
                        command.Parameters.AddWithValue("@Num_Factura", compra.Num_Factura);
                        command.Parameters.AddWithValue("@FechaCompra", compra.FechaCompra);
                        command.Parameters.AddWithValue("@Subtotal", compra.SubTotal);
                        command.Parameters.AddWithValue("@IVA", compra.IVA);
                        command.Parameters.AddWithValue("@Total", compra.Total);

                        // Crear la tabla para los detalles de la compra                       
                        DataTable detalleTable = new DataTable();
                        detalleTable.Columns.Add("Id_DetalleProducto", typeof(int));
                        detalleTable.Columns.Add("Precio_Compra", typeof(decimal));
                        detalleTable.Columns.Add("Cantidad", typeof(int));
                        detalleTable.Columns.Add("Subtotal", typeof(decimal));

                        foreach (var detalle in compra.DetalleCompra)
                        {
                            detalleTable.Rows.Add(detalle.Id_DetalleProducto, detalle.Precio_Compra, detalle.Cantidad, detalle.SubTotal);
                        }

                        // Agregar parámetro para los detalles
                        SqlParameter detalles = new SqlParameter("@Detalles", SqlDbType.Structured)
                        {
                            Value = detalleTable
                        };
                        command.Parameters.Add(detalles);

                        // Ejecutar el procedimiento almacenado
                        var result = command.ExecuteScalar().ToString();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }
}
