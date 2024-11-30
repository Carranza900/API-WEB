namespace Paletitas.Models
{
    public class VentasDtoSoli
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string Num_Factura { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public List<DetalleVentaSoli> DetalleVenta { get; set; } = new List<DetalleVentaSoli>();

    }
}
