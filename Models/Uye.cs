using System.ComponentModel.DataAnnotations;

namespace HaberSitesi.Models
{
    public class Uye
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? AdSoyad { get; set; }

        [Required]
        [EmailAddress]
        public string? Eposta { get; set; }

        [Required]
        public string? Sifre { get; set; }

        public string? Rol { get; set; } = "Uye";
    }
}