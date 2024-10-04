namespace Integrador.Models
{
    public class ProductoDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string EstadoTexto { get; set; }
        public string NombreCategoria { get; set; }
        public bool Estado { get; set; }
        public int Id_Categoria { get; set; } 
    }
}
