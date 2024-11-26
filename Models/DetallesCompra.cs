namespace SISWIN.Models
{
    public class DetallesCompra
    {
        public int ID_Detalle_Compra { get; set; }
        public int Id_Compra { get; set; }     
        public int Id_DetalleProducto { get; set; }
        public Decimal Precio_Compra { get; set; }
        public int Cantidad { get; set; }
        public Decimal SubTotal { get; set; }
        
    }
}
