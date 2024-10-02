using Microsoft.Data.SqlClient;
using Integrador.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Integrador.DataAccess
{
    public class ClienteDAL
    {
        private readonly string cadenaConexion;

        // Inyectar IConfiguration para obtener la cadena de conexión
        public ClienteDAL(IConfiguration configuration)
        {
            // Obtener la cadena de conexión del archivo appsettings.json
            cadenaConexion = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Cliente> ListarClientes()
        {
            List<Cliente> listaclientes = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ListarClientes", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cliente clientes = new Cliente()
                        {
                            ID_Cliente = (int)reader["ID_Cliente"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Telefono = reader["Telefono"].ToString()
                        };
                        listaclientes.Add(clientes);
                    }
                }
            }

            return listaclientes;
        }

        public int InsertarCliente(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertarCliente", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                var idCliente = command.ExecuteScalar();
                return Convert.ToInt32(idCliente);
            }
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ActualizarCliente", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Cliente", cliente.ID_Cliente);
                command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        public bool EliminarCliente(int IdCliente)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("EliminarCliente", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Cliente", IdCliente);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }
    }
}
