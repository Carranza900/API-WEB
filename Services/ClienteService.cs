using Integrador.Models;
using Integrador.DataAccess;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Integrador.Services
{
    public class ClienteService
    {
        private readonly ClienteDAL _clienteDAL;


        public ClienteService(IConfiguration configuration)
        {
            _clienteDAL = new ClienteDAL(configuration);
        }

        public List<Cliente> ObtenerClientes()
        {
            return _clienteDAL.ListarClientes();
        }

        public int AgregarCliente(Cliente cliente)
        {
            return _clienteDAL.InsertarCliente(cliente);
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            return _clienteDAL.ActualizarCliente(cliente);
        }

        public bool EliminarCliente(int IdCliente)
        {
            return _clienteDAL.EliminarCliente(IdCliente);
        }
    }
}
