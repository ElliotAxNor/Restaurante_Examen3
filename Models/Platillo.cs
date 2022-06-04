using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models
{
    public class Platillo
    {
		[Key]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Nombre del platillo")]
		public string Nombre { get; set; }

		[Display(Name = "Categoria")]
		public int IdCategoria { get; set; }

		[ForeignKey("IdCategoria")]
		public Categoria? Categoria { get; set; }

		[Required]
		[Display(Name = "Descripcion del platillo")]
		public string Descripcion { get; set; }

		[Required]
		[Display(Name = "Precio del platillo")]
		public double Precio { get; set; }
		[Display(Name = "Imagen del platillo")]
		public string? UrlImagen { get; set; }
	}
}
