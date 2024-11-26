using System.Collections.Generic;
using Paletitas.DataAccess;
using Paletitas.Models;

namespace Paletitas.Services
{
    public class ProveedorServices
    {
        private readonly ProveedorDAL _proveedorDAL;

        public ProveedorServices(string connectionString)
        {
            _proveedorDAL = new ProveedorDAL(connectionString);
        }

        // Método para agregar un proveedor
        public void AgregarProveedor(Proveedor proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.NumRuc))
                throw new ArgumentException("El RUC no puede estar vacío.");
            if (proveedor.NumRuc.Length != 11)
                throw new ArgumentException("El RUC debe tener 11 caracteres.");
            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
                throw new ArgumentException("La razón social no puede estar vacía.");

            _proveedorDAL.InsertarProveedor(proveedor);
        }

        // Método para listar todos los proveedores
        public List<Proveedor> ObtenerProveedores()
        {
            return _proveedorDAL.ListarProveedores();
        }

        // Método para actualizar un proveedor
        public void ActualizarProveedor(Proveedor proveedor)
        {
            if (proveedor.ID_Proveedor <= 0)
                throw new ArgumentException("El ID del proveedor no es válido.");
            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new ArgumentException("El nombre del proveedor no puede estar vacío.");

            _proveedorDAL.ActualizarProveedor(proveedor);
        }

        // Método para eliminar un proveedor
        public void EliminarProveedor(int idProveedor)
        {
            if (idProveedor <= 0)
                throw new ArgumentException("El ID del proveedor no es válido.");

            _proveedorDAL.EliminarProveedor(idProveedor);
        }
    }
}
