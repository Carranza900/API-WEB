namespace Paletitas.Models
{
    public class VentasDtoRes
    {
        public int IDVenta{ get; set; }
        public string NumFactura { get; set; }
        public string Cliente { get; set; }
        public string Usuario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaVenta { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; }
    }

    public class DetalleVentaDto
    {
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }


}

