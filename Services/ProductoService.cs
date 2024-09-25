using Integrador.Models;
using ProyectoIV.DataAccess;
using System.Collections.Generic;

namespace ProyectoIV.Services
{
    public class ProductoService
    {
        private ProductoDAL productoDAL = new ProductoDAL();

        public List<Producto> ObtenerProductos()
        {
            return productoDAL.ListarProductos();
        }

        public int AgregarProducto(Producto producto)
        {
            return productoDAL.InsertarProducto(producto);
        }

        public bool ActualizarProducto(Producto producto)
        {
            return productoDAL.ActualizarProducto(producto);
        }

        public bool EliminarProducto(int idProducto)
        {
            return productoDAL.EliminarProducto(idProducto);
        }
    }
}
