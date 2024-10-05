using Integrador.Models;
using System;
using System.Collections.Generic;
using Integrador.DataAccess;
using ProyectoIV.DataAccess;

namespace Integrador.Services
{
    public class VentaService
    {
        private readonly VentaDAL _ventaDAL;

        public IEnumerable<Ventas> GetVentas()
        {
            return _ventaDAL.GetVentas();
        }

        public int CrearVenta(Ventas venta)
        {
            return _ventaDAL.InsertarVenta(venta);

        }

    }
}
