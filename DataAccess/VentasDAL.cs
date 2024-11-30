using System.Data;
using System.Data.SqlClient;
using Paletitas.Models;


namespace Paletitas.DataAccess
{
    public class VentasDAL
    {

        private readonly string _connectionString;

        public VentasDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<VentasDtoRes> GetAll()
        {
            var ventas = new List<VentasDtoRes>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ListarVentas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentasDtoRes ventasDto = new VentasDtoRes
                            {
                                IDVenta = (int)reader["ID_Venta"],
                                Cliente = reader.GetString(reader.GetOrdinal("Cliente")),
                                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                                NumFactura = reader["Num_Factura"].ToString(),
                                FechaVenta = (DateTime)reader["FechaVenta"],
                                Subtotal = (decimal)reader["subtotal"],
                                IVA = (decimal)reader["IVA"],
                                Total = (decimal)reader["Total"],
                                Detalles = new List<DetalleVentaDto>()
                            };
                            ventas.Add(ventasDto);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                int idVenta = reader.GetInt32(reader.GetOrdinal("Id_Venta"));
                                var venta = ventas.FirstOrDefault(v => v.IDVenta == idVenta);

                                if (venta != null)
                                {
                                    var detalle = new DetalleVentaDto
                                    {
                                        Producto = reader.GetString(reader.GetOrdinal("Producto")),
                                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                        Subtotal = reader.GetDecimal(reader.GetOrdinal("DetalleSubtotal"))
                                    };

                                    venta.Detalles.Add(detalle);
                                }
                            }
                        }
                    }

                }
            }
            return ventas;
        }

        public VentasDtoSoli Add(VentasDtoSoli venta)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand cd = new SqlCommand("sp_CrearVenta", connection))
                    {

                        cd.CommandType = CommandType.StoredProcedure;
                        cd.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                        cd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                        cd.Parameters.AddWithValue("NumFac", venta.Num_Factura);
                        cd.Parameters.AddWithValue("@Fecha", venta.FechaVenta);
                        cd.Parameters.AddWithValue("@Subtotal", venta.Subtotal);
                        cd.Parameters.AddWithValue("@Iva ", venta.IVA);
                        cd.Parameters.AddWithValue("@Total", venta.Total);


                        DataTable detalleTable = new DataTable();
                        detalleTable.Columns.Add("Id_Producto", typeof(int));
                        detalleTable.Columns.Add("Precio", typeof(decimal));
                        detalleTable.Columns.Add("Cantidad", typeof(int));
                        detalleTable.Columns.Add("Subtotal", typeof(decimal));

                        foreach (var detalle in venta.DetalleVenta)
                        {
                            detalleTable.Rows.Add(detalle.IdProducto, detalle.Precio, detalle.Cantidad, detalle.Subtotal);
                        }

                        SqlParameter detalleParameter = new SqlParameter("@Detalles", SqlDbType.Structured)
                        {
                            TypeName = "dbo.TDetalleVenta",
                            Value = detalleTable
                        };
                        cd.Parameters.Add(detalleParameter);

                        cd.ExecuteNonQuery();

                        return venta;
                    }
                }
                catch (SqlException ex)
                {

                    if (ex.Number == 50003)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return null;
                    }
                    else
                    {

                        Console.WriteLine("Error inesperado: " + ex.Message);
                        return null;
                    }
                }
                
            }
        }

        public List<VentasDtoRes> GetById(int id)
        {
            List<VentasDtoRes> venta = new List<VentasDtoRes>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ListarVentasId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVenta", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var ventas = new VentasDtoRes
                            {

                                IDVenta = (int)reader["ID_Venta"],
                                Cliente = reader.GetString(reader.GetOrdinal("Cliente")),
                                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                                NumFactura = reader["Num_Factura"].ToString(),
                                FechaVenta = (DateTime)reader["FechaVenta"],
                                Subtotal = (decimal)reader["subtotal"],
                                IVA = (decimal)reader["IVA"],
                                Total = (decimal)reader["Total"],
                                Detalles = new List<DetalleVentaDto>()
                            };
                            venta.Add(ventas);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                int idVenta = reader.GetInt32(reader.GetOrdinal("Id_Venta"));

                                foreach (var ventas in venta)
                                {
                                    if (ventas.IDVenta == idVenta)
                                    {
                                        var detalle = new DetalleVentaDto
                                        {
                                            Producto = reader.GetString(reader.GetOrdinal("Producto")),
                                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                            Subtotal = reader.GetDecimal(reader.GetOrdinal("DetalleSubtotal"))
                                        };

                                        ventas.Detalles.Add(detalle);
                                        break;
                                    }
                                }
                            }
                        }


                    }
                }
                return venta;
            }

        }    
        public string UpdateVenta(int id,VentasDtoSoli venta)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("sp_ActualizarVenta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdVenta", id);
                        command.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                        command.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                        command.Parameters.AddWithValue("@NumFac", venta.Num_Factura);
                        command.Parameters.AddWithValue("@Fecha", venta.FechaVenta);
                        command.Parameters.AddWithValue("@Subtotal", venta.Subtotal);
                        command.Parameters.AddWithValue("@IVA", venta.IVA);
                        command.Parameters.AddWithValue("@Total", venta.Total);


                        var detallesTable = new DataTable();
                        detallesTable.Columns.Add("Id_Producto", typeof(int));
                        detallesTable.Columns.Add("Precio", typeof(decimal));
                        detallesTable.Columns.Add("Cantidad", typeof(int));
                        detallesTable.Columns.Add("Subtotal", typeof(decimal));

                        foreach (var detalle in venta.DetalleVenta)
                        {
                            detallesTable.Rows.Add(detalle.IdProducto, detalle.Precio, detalle.Cantidad, detalle.Subtotal);
                        }

                        SqlParameter detalles = new SqlParameter("@Detalles", SqlDbType.Structured)
                        {
                            Value = detallesTable
                        };
                        command.Parameters.Add(detalles);

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

    

 







