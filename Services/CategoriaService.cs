using Integrador.Models;
using ProyectoIV.DataAccess;
using System.Collections.Generic;

namespace ProyectoIV.Services
{
    public class CategoriaService
    {
        private CategoriaDAL categoriaDAL = new CategoriaDAL();

        public List<Categoria> ObtenerCategorias()
        {
            return categoriaDAL.ListarCategorias();
        }

        public bool AgregarCategoria(Categoria categoria)
        {
            return categoriaDAL.InsertarCategoria(categoria);
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            return categoriaDAL.ActualizarCategoria(categoria);
        }

        public bool EliminarCategoria(int idCategoria)
        {
            return categoriaDAL.EliminarCategoria(idCategoria);
        }
    }
}
