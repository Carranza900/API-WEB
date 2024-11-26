using Paletitas.DataAccess;
using Paletitas.Models;
using System.Collections.Generic;

namespace Paletitas.Services
{
    public class CategoriaServices
    {
        private readonly CategoriaDAL _categoriaDAL;

        public CategoriaServices(string connectionString)
        {
            _categoriaDAL = new CategoriaDAL(connectionString);
        }

        // Método para obtener la lista de categorías
        public List<Categoria> ObtenerCategorias()
        {
            return _categoriaDAL.ListarCategorias();
        }

        // Método para agregar una nueva categoría
        public void AgregarCategoria(string nombre, string descripcion, bool estado)
        {
            _categoriaDAL.InsertarCategoria(nombre, descripcion, estado);
        }

        // Método para actualizar una categoría existente
        public void ActualizarCategoria(int idCategoria, string nombre, string descripcion, bool estado)
        {
            _categoriaDAL.ActualizarCategoria(idCategoria, nombre, descripcion, estado);
        }

        // Método para eliminar una categoría por ID
        public void EliminarCategoria(int idCategoria)
        {
            _categoriaDAL.EliminarCategoria(idCategoria);
        }
    }
}
