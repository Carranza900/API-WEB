using Paletitas.Models;
using System.Data;
using System.Reflection.PortableExecutable;
using Paletitas.DataAccess;

namespace Paletitas.Services
{
    public class ProductoService
    {

            private readonly ProductoDAL _productoDAL;

            public ProductoService(string connectionString)
            {
                _productoDAL = new ProductoDAL(connectionString);
            }

            public List<Producto> ObtenerProductos()
            {
                return _productoDAL.ListarProductos();
            }

            public void InsertarProductoConDetallesYCaracteristicas(int idCategoria, string nombre, string descripcion, bool estado, List<DetalleProducto> detalles, List<Caracteristica> caracteristicas)
            {
                var detallesTable = CrearDataTableDetalles(detalles);
                var caracteristicasTable = CrearDataTableCaracteristicas(caracteristicas);

                _productoDAL.InsertarProducto(idCategoria, nombre, descripcion, estado, detallesTable, caracteristicasTable);
            }

            private DataTable CrearDataTableDetalles(List<DetalleProducto> detalles)
            {
                var detallesTable = new DataTable();
                detallesTable.Columns.Add("Id_Ubicacion", typeof(int));
                detallesTable.Columns.Add("Existencia", typeof(int));
                detallesTable.Columns.Add("Precio_Compra", typeof(decimal));
                detallesTable.Columns.Add("Precio_Venta", typeof(decimal));

                foreach (var detalle in detalles)
                {
                    detallesTable.Rows.Add(detalle.IdUbicacion, detalle.Existencia, detalle.PrecioCompra, detalle.PrecioVenta);
                }
                return detallesTable;
            }

            private DataTable CrearDataTableCaracteristicas(List<Caracteristica> caracteristicas)
            {
                var caracteristicasTable = new DataTable();
                caracteristicasTable.Columns.Add("Id_Caracteristica", typeof(int));

                foreach (var caracteristica in caracteristicas)
                {
                    caracteristicasTable.Rows.Add(caracteristica.IdCaracteristica);
                }

                return caracteristicasTable;
            }
        
    }
}
