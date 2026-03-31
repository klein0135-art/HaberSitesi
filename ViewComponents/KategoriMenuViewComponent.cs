using System.Threading.Tasks;
using HaberSitesi.Interfaces;
using HaberSitesi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HaberSitesi.ViewComponents
{
    public class KategoriMenuViewComponent : ViewComponent
    {
        private readonly IRepository<Kategori> _kategoriRepo;

        public KategoriMenuViewComponent(IRepository<Kategori> kategoriRepo)
        {
            _kategoriRepo = kategoriRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kategoriler = _kategoriRepo.GetAll() ?? new List<Kategori>();
            return View(kategoriler);
        }
    }
}
