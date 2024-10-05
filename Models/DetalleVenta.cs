
namespace Integrador.Models
{
    public class DetalleVenta
    {


        public int ID_Detalle_Venta { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Producto { get; set; }
        public decimal Precio_Unitario { get; set; }
        public int Cantidad { get; set; }




    }
}
