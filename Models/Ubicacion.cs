namespace SISWIN.Models
{
    public class Ubicacion
    {
        public int ID_Ubicacion { get; set; }
        public int ID_Almacen { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Relación con Almacen
        public Almacen Almacen { get; set; }
    }
}
