
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

        public VentasDtoSoli Add(VentasDtoSoli venta)
        {
            return _ventaDAL.Add(venta);
        }

        public List<VentasDtoRes> GetAll()
        {
            return _ventaDAL.GetAll();
        }

        public List<VentasDtoRes> GetById(int id)
        {
            var venta = _ventaDAL.GetById(id);
                
            if (venta == null)
            {
                Console.WriteLine($"Venta con ID {id} no encontrada en la capa de servicios.");
            }

            return venta;
            
        }
        public string UpdateVenta(int id,VentasDtoSoli venta)
        {
            return _ventaDAL.UpdateVenta(id,venta);
        }

    }

}

