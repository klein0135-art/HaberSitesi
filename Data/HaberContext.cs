using HaberSitesi.Models;
using Microsoft.EntityFrameworkCore;

namespace HaberSitesi.Data
{
    public class HaberContext : DbContext
    {
        public HaberContext(DbContextOptions<HaberContext> options) : base(options)
        {
        }

        public DbSet<Haber> Haberler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }

        public DbSet<Uye> Uyeler { get; set; }

        public DbSet<Yorum> Yorumlar { get; set; }
    }
}