namespace Paletitas.Models
{
    public class Ventas
    {
        public int ID { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string Num_Factura { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public List<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
    }
}
