using Integrador.Models;
using Integrador.DataAccess;
using System.Collections.Generic;

namespace Integrador.Services
{
    public class ClienteService
    {
        private ClienteDAL CLIENTEDAL = new ClienteDAL();

        public List<Cliente> ObtenerClientes()
        {
            return CLIENTEDAL.ListarClientes();
        }

        public int AgregarCliente (Cliente cliente)
        {
            return CLIENTEDAL.InsertarCliente(cliente);
        }

        public bool ActualizarCliente (Cliente cliente)
        {
            return CLIENTEDAL.ActualizarCliente(cliente);
        }

        public bool EliminarCliente (int IdCliente)
        {
            return CLIENTEDAL.EliminarCliente(IdCliente);
        }



    }




}

