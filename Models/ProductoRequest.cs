using System.Collections.Generic;

namespace SISWIN.Models
{
    public class ProductoRequest
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public List<DetalleProducto> Detalles { get; set; } // Lista de detalles del producto
        public List<Caracteristica> Caracteristicas { get; set; } // Lista de características
    }


    public class Caracteristica
    {
        public int IdCaracteristica { get; set; }  // Propiedad que falta
        public string Descripcion { get; set; }
    }
}
