using System.ComponentModel.DataAnnotations;

namespace HaberSitesi.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Kategori Adı Boş Geçilemez!")]
        [StringLength(50 , ErrorMessage ="Kategori adı en fazla 50 karakter olabilir")]
        [Display(Name ="Kategori Adı")]
        public string? Ad { get; set; }

        public virtual List<Haber>? Haberler { get; set; }
    }
}