using Integrador.Models;
using ProyectoIV.DataAccess;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ProyectoIV.Services
{
    public class ProductoService
    {
        private readonly ProductoDAL _productoDAL;

        public ProductoService(IConfiguration configuration)
        {
            _productoDAL = new ProductoDAL(configuration);
        }

        public List<Producto> ObtenerProductos()
        {
            return _productoDAL.ListarProductos();
        }

        public int AgregarProducto(Producto producto)
        {
            return _productoDAL.InsertarProducto(producto);
        }

        public bool ActualizarProducto(Producto producto)
        {
            return _productoDAL.ActualizarProducto(producto);
        }

        public bool EliminarProducto(int idProducto)
        {
            return _productoDAL.EliminarProducto(idProducto);
        }
    }
}
