namespace Paletitas.Models
{
    public class Devolucion
    {
        public int ID_Devolucion { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Producto { get; set; }
        public string EstadoProducto { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha_Devolucion { get; set; }

        // Navigation properties
        public Usuarios Usuario { get; set; }
        public Ventas Venta { get; set; }
        public Producto Producto { get; set; }
    }
}
