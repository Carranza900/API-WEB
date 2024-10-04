using Integrador.DataAccess;
using Integrador.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Integrador.Services
{
    public class ProveedorService
    {
        private readonly ProveedorDAL proveedorDAL;

        public ProveedorService(IConfiguration configuration)
        {
            proveedorDAL = new ProveedorDAL(configuration);
        }

        public List<Proveedor> ObtenerProveedores()
        {
            return proveedorDAL.ListarProveedores();
        }

        public int AgregarProveedor(Proveedor proveedor)
        {
            return proveedorDAL.InsertarProveedor(proveedor);
        }

        public bool ActualizarProveedor(Proveedor proveedor)
        {
            return proveedorDAL.ActualizarProveedor(proveedor);
        }

        public bool EliminarProveedor(int idProveedor)
        {
            return proveedorDAL.EliminarProveedor(idProveedor);
        }
    }
}
