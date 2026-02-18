using System.ComponentModel.DataAnnotations;

namespace HaberSitesi.Models
{
    public class Yorum
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ad Soyad Alanı Boş Bırakılamaz")]
        [StringLength(50 , ErrorMessage = "İsim Çok Uzun")]
        public string? AdSoyad { get; set; }

        [Required (ErrorMessage ="Yorum yazmalısın")]
        [StringLength(600 , ErrorMessage ="Yorum 500 Karakteri Geçemez")]
        public string? Icerik { get; set; }

        public DateTime Tarih {  get; set; } = DateTime.Now;

        public int HaberId { get; set; }

        public virtual Haber? Haber { get; set; }
    }
}
