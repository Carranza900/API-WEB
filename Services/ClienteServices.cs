using System;
using System.Collections.Generic;
using Paletitas.DataAccess;
using Paletitas.Models;


namespace Paletitas.Services
{
    public class ClienteServices
    {
        private readonly ClienteDAL _clienteDAL;

        public ClienteServices(string connectionString)
        {
            _clienteDAL = new ClienteDAL(connectionString);
        }

        // Método para agregar un cliente
        public void AgregarCliente(string nombre, string apellido, string telefono)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono no puede estar vacío.");

            _clienteDAL.InsertarCliente(nombre, apellido, telefono);
        }

        // Método para obtener la lista de clientes
        public List<Cliente> ObtenerClientes()
        {
            return _clienteDAL.ListarClientes();
        }

        // Método para actualizar un cliente
        public void ActualizarCliente(int idCliente, string nombre, string apellido, string telefono)
        {
            if (idCliente <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor que cero.");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono no puede estar vacío.");

            _clienteDAL.ActualizarCliente(idCliente, nombre, apellido, telefono);
        }

        // Método para eliminar un cliente
        public void EliminarCliente(int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor que cero.");

            _clienteDAL.EliminarCliente(idCliente);
        }
    }
}
