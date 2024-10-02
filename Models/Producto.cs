namespace Integrador.Models
{
    public class Producto
    {
        public int ID_Producto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int Id_Categoria { get; set; }
        public string NombreCategoria { get; set; }
    }
}
