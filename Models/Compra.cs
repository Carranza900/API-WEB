namespace SISWIN.Models
{
    public class Compra
    {
        public int ID_Compra { get; set; }
        public int ID_Proveedor { get; set; }
        public int ID_Usuario { get; set; }
        public string Num_Factura { get; set; }
        public DateTime FechaCompra { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal IVA {  get; set; }
        public Decimal Total { get; set; }
        public List<DetalleCompra> DetalleCompra { get; set; } = new List<DetalleCompra>();

    }
}
