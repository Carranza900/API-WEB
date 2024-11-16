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
                                ID_Proveedor = (int)reader["Id_Proveedor"],
                                ID_Usuario = (int)reader["Id_Usuario"],
                                Num_Factura = reader["Num_Factura"].ToString(),
                                FechaCompra = (DateTime)reader["FechaCompra"],
                                SubTotal = (decimal)reader["Subtotal"],
                                IVA = (decimal)reader["IVA"],
                                Total = (decimal)reader["Total"],
                                DetalleCompra = new List<DetalleCompra>()
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
                        var detalle = new DetalleCompra
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
                    using (SqlCommand cd = new SqlCommand("sp_CrearCompra", connection))
                    {
                        cd.CommandType = CommandType.StoredProcedure;
                        cd.Parameters.AddWithValue("@IdProveedor", compra.ID_Proveedor);
                        cd.Parameters.AddWithValue("@IdUsuario", compra.ID_Usuario);
                        cd.Parameters.AddWithValue("@NumFactura", compra.Num_Factura);
                        cd.Parameters.AddWithValue("@FechaCompra", compra.FechaCompra);
                        cd.Parameters.AddWithValue("@Subtotal", compra.SubTotal);
                        cd.Parameters.AddWithValue("@IVA", compra.IVA);
                        cd.Parameters.AddWithValue("@Total", compra.Total);

                        // Crear la tabla para los detalles
                        DataTable detalleTable = new DataTable();
                        detalleTable.Columns.Add("Id_DetalleProducto", typeof(int));
                        detalleTable.Columns.Add("Precio_Compra", typeof(decimal));
                        detalleTable.Columns.Add("Cantidad", typeof(int));
                        detalleTable.Columns.Add("Subtotal", typeof(decimal));

                        foreach (var detalle in compra.DetalleCompra)
                        {
                            detalleTable.Rows.Add(detalle.Id_DetalleProducto, detalle.Precio_Compra, detalle.Cantidad, detalle.SubTotal);
                        }

                        SqlParameter detalleParameter = new SqlParameter("@DetalleCompra", SqlDbType.Structured)
                        {
                            TypeName = "dbo.TDetalleCompra",
                            Value = detalleTable
                        };
                        cd.Parameters.Add(detalleParameter);

                        cd.ExecuteNonQuery();

                        return compra;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
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
                            compra.ID_Proveedor = reader.GetInt32(1);
                            compra.ID_Usuario = reader.GetInt32(2);
                            compra.Num_Factura = reader.GetString(3);
                            compra.FechaCompra = reader.GetDateTime(4);
                            compra.SubTotal = reader.GetDecimal(5);
                            compra.IVA = reader.GetDecimal(6);
                            compra.Total = reader.GetDecimal(7);
                        }
                    }
                }

                // Consulta para obtener los detalles de la compra
                using (var command = new SqlCommand("SELECT * FROM DetalleCompra WHERE Id_Compra = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        compra.DetalleCompra = new List<DetalleCompra>();
                        while (reader.Read())
                        {
                            compra.DetalleCompra.Add(new DetalleCompra
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


    }
}
