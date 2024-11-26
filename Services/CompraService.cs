using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SISWIN.DataAccess;
using SISWIN.Models;

namespace SISWIN.Services
{
    public class CompraService
    {
        private readonly CompraDAL _compraDAL;

        public CompraService(string connectionString)
        {
            _compraDAL = new CompraDAL(connectionString);
        }

            public List<Compra> GetAll()
            {
                return _compraDAL.GetAll();
            }

            public Compra GetById(int id)
            {
                if (id <= 0)
                    throw new ArgumentException("El ID de la compra debe ser mayor a cero.");

                return _compraDAL.GetById(id);
            }

            public Compra Add(Compra compra)
            {
                if (compra == null || compra.DetalleCompra == null || !compra.DetalleCompra.Any())
                    throw new ArgumentException("La compra o sus detalles no son válidos.");

                return _compraDAL.Add(compra);
            }

             public string UpdateCompra(Compra compra)
             {
                 return _compraDAL.UpdateCompra(compra);
             }

    }
}
