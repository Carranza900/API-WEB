namespace SISWIN.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string EstadoProducto { get; set; }
        public string Categoria { get; set; }
        public string Almacen { get; set; }
        public string Ubicacion { get; set; }
        public int Existencia { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public List<string> Caracteristicas { get; set; } = new List<string>();
    }
}
