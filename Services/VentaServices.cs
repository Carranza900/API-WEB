
using Paletitas.DataAccess;
using Paletitas.Models;

namespace Paletitas.Services
{
    public class VentaServices
    {
        private readonly VentasDAL _ventaDAL;

        public VentaServices(string connectionString)
        {
            _ventaDAL = new VentasDAL(connectionString);
        }

        public Ventas Add(Ventas venta)
        {
            return _ventaDAL.Add(venta);
        }

        public List<VentasDto> GetAll()
        {
            return _ventaDAL.GetAll();
        }

        public List<VentasDto> GetById(int id)
        {
            var venta = _ventaDAL.GetById(id);
                
            if (venta == null)
            {
                Console.WriteLine($"Venta con ID {id} no encontrada en la capa de servicios.");
            }

            return venta;
            
        }
        public string UpdateVenta(Ventas venta)
        {
            return _ventaDAL.UpdateVenta(venta);
        }

    }

}

