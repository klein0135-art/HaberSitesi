using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace HaberSitesi.Models
{
    public class Haber
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Başlık Zorunludur")]
        [StringLength(200 , ErrorMessage ="Başlık 200 karakteri geçemez")]
        public string? Baslik { get; set; }
        [Required(ErrorMessage ="İçerik Zorunludur")]
        public string? Icerik { get; set; }

        public string? ResimYolu { get; set; }

        [NotMapped]
        [Display(Name ="resim yükle")]
        public IFormFile ResimDosyasi { get; set; }
        public DateTime Tarih {  get; set; }

        public int KategoriId { get; set; }

        public virtual Kategori? Kategori { get; set; }

        public int OkunmaSayisi { get; set; } = 0;
    }
}