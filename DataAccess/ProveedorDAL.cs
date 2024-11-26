using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Paletitas.Models;

namespace Paletitas.DataAccess
{
    public class ProveedorDAL
    {
        private readonly string _connectionString;

        public ProveedorDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para insertar un proveedor
        public void InsertarProveedor(Proveedor proveedor)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("insertar_proveedor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NumRuc", proveedor.NumRuc);
                    command.Parameters.AddWithValue("@Razon_Social", proveedor.RazonSocial);
                    command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    command.Parameters.AddWithValue("@Apellido", proveedor.Apellido);
                    command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    command.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                    command.Parameters.AddWithValue("@Correo", proveedor.Correo);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // Método para listar proveedores
        public List<Proveedor> ListarProveedores()
        {
            var proveedores = new List<Proveedor>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("listar_proveedores", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(new Proveedor
                            {
                                ID_Proveedor = Convert.ToInt32(reader["ID_Proveedor"]),
                                NumRuc = reader["NumRuc"].ToString(),
                                RazonSocial = reader["Razon_Social"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Correo = reader["Correo"].ToString()
                            });
                        }
                    }
                }
            }

            return proveedores;
        }

        // Método para actualizar un proveedor
        public void ActualizarProveedor(Proveedor proveedor)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("actualizar_proveedor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Proveedor", proveedor.ID_Proveedor);
                    command.Parameters.AddWithValue("@NumRuc", proveedor.NumRuc);
                    command.Parameters.AddWithValue("@Razon_Social", proveedor.RazonSocial);
                    command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    command.Parameters.AddWithValue("@Apellido", proveedor.Apellido);
                    command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    command.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                    command.Parameters.AddWithValue("@Correo", proveedor.Correo);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // Método para eliminar un proveedor
        public void EliminarProveedor(int idProveedor)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("eliminar_proveedor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Proveedor", idProveedor);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
