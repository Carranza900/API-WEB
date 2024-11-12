namespace SISWIN.Models
{
    public class DetalleProductoAlmacenType
    {
        public int Id_Ubicacion { get; set; }
        public int Id_Proveedor { get; set; }
        public int Id_Lote { get; set; }
        public int Existencia { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Venta { get; set; }

    }
}
