using Integrador.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Net.Sockets;
using System.Data;

namespace Integrador.DataAccess
{
    public class VentaDAL
    {
        private readonly string _cadenaConexion;
        public VentaDAL(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Ventas> GetVentas()//OBTENER LAS VENTAS
        {
            List<Ventas> ventas = new List<Ventas>();
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from Ventas", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Ventas venta = new Ventas
                    {
                        ID_Venta = (int)reader["ID_Venta"],
                        Id_Cliente = (int)reader["Id_Cliente"],
                        Id_Usuario = (int)reader["Id_Usuario"],
                        Num_Factura = (int)reader["Num_Factura"],
                        Fecha_Venta = (DateTime)reader["Fecha_Venta"],
                        Subtotal = (decimal)reader["Subtotal"],
                        IVA = (decimal)reader["IVA"],
                        Total = (decimal)reader["Total"]
                    };
                    ventas.Add(venta);
                }
            }
            return ventas;
        }

        public int InsertarVenta(Ventas venta)
        {
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertarVenta", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id_Cliente", venta.Id_Cliente);
                command.Parameters.AddWithValue("@Id_Usuario", venta.Id_Usuario);
                command.Parameters.AddWithValue("@Num_Factura", venta.Num_Factura);
                command.Parameters.AddWithValue("@Fecha_Venta", venta.Fecha_Venta);
                command.Parameters.AddWithValue("@Subtotal ", venta.Subtotal);
                command.Parameters.AddWithValue("@IVA", venta.IVA);
                command.Parameters.AddWithValue("@Total", venta.Total);

                var Id_Venta = command.ExecuteScalar();
                return Convert.ToInt32(Id_Venta);
            }
        }





    }
}       
