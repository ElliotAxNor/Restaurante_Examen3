using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre de la categoria")]
        public string Nombre { get; set; }

        [Display(Name = "Imagen de la categoria")]
        public string? UrlImagen { get; set; }
    }
}
