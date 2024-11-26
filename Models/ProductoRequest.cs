using System.Reflection.PortableExecutable;

namespace Paletitas.Models
{
    public class ProductoRequest
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public List<DetalleProducto> Detalles { get; set; }
        public List<Caracteristica> Caracteristicas { get; set; }

    }
    
}
