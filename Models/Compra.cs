using System.Text.Json.Serialization;

namespace SISWIN.Models
{
    public class Compra
    {
        public int ID_Compra { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreProducto { get; set; }
        public string Num_Factura { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal IVA {  get; set; }
        public Decimal Total { get; set; }

        [JsonIgnore] // Esto oculta la propiedad en la salida JSON.
        public List<DetallesCompra> DetalleCompra { get; set; } = new List<DetallesCompra>();

    }
}
