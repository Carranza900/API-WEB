namespace Paletitas.Models
{
    public class DetalleProducto
    {
        public int IdUbicacion { get; set; }
        public int IdProveedor { get; set; }
        public int IdLote { get; set; }
        public int Existencia { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}
