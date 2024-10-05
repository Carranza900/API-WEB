
using System.Runtime.CompilerServices;

namespace Integrador.Models
{
    public class Ventas
    {
       public int ID_Venta { get; set; }
        public int Id_Cliente { get; set; }
		public int Id_Usuario { get; set; }
        public int Num_Factura { get; set; }
		public DateTime Fecha_Venta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public List <DetalleVenta> lstDetalleVenta { get; set; }
 
        public Ventas()
        {
            this.lstDetalleVenta = new List<DetalleVenta>();
        }



    }
}
