using Integrador.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Integrador.DataAccess
{
    public class ProveedorDAL
    {
        private readonly string _cadenaConexion;

        // Constructor que inyecta la configuración
        public ProveedorDAL(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("DefaultConnection");
        }

        // MÉTODO PARA LISTAR PROVEEDORES 
        public List<Proveedor> ListarProveedores()
        {
            List<Proveedor> listaProveedores = new List<Proveedor>();

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("ListarProveedores", conexion);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Proveedor proveedor = new Proveedor
                        {
                            ID_Proveedor = (int)reader["ID_Proveedor"],
                            Empresa = reader["Empresa"].ToString(),
                            Contacto = reader["Contacto"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Correo = reader["Correo"].ToString()
                        };
                        listaProveedores.Add(proveedor);
                    }
                }
            }
            return listaProveedores;
        }

        // MÉTODO PARA INSERTAR PROVEEDORES 
        public int InsertarProveedor(Proveedor proveedor)
        {
            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("InsertarProveedor", conexion);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Empresa", proveedor.Empresa);
                command.Parameters.AddWithValue("@Contacto", proveedor.Contacto);
                command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                command.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                command.Parameters.AddWithValue("@Correo", proveedor.Correo);

                command.ExecuteNonQuery(); 
                return 1; 
            }

        }

        // MÉTODO PARA ACTUALIZAR PROVEEDOR 
        public bool ActualizarProveedor(Proveedor proveedor)
        {
            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("ActualizarProveedor", conexion);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_Proveedor", proveedor.ID_Proveedor);
                command.Parameters.AddWithValue("@Empresa", proveedor.Empresa);
                command.Parameters.AddWithValue("@Contacto", proveedor.Contacto);
                command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                command.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                command.Parameters.AddWithValue("@Correo", proveedor.Correo);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

        // MÉTODO PARA ELIMINAR PROVEEDORES 
        public bool EliminarProveedor(int idProveedor)
        {
            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                SqlCommand command = new SqlCommand("EliminarProveedor", conexion);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("ID_Proveedor", idProveedor);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }
    }
}
