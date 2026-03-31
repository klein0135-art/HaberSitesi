using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HaberSitesi.Models;
using HaberSitesi.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace HaberSitesi.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IRepository<Haber> _haberRepo;
        private readonly IRepository<Kategori> _kategoriRepo;
        private readonly IRepository<Yorum> _yorumRepo;

      
        public HomeController(IRepository<Haber> haberRepo, IRepository<Kategori> kategoriRepo, IRepository<Yorum> yorumRepo)
        {
            _haberRepo = haberRepo;
            _kategoriRepo = kategoriRepo;
            _yorumRepo = yorumRepo;
        }

      
        public IActionResult Index()
        {
           
            ViewBag.Populer = _haberRepo.GetAll()
                                        .OrderByDescending(x => x.OkunmaSayisi)
                                        .Take(5)
                                        .ToList() ?? new List<Haber>();

         
            var sonHaberler = _haberRepo.GetAll(x => x.Kategori)
                                        .OrderByDescending(h => h.Tarih)
                                        .Take(10)
                                        .ToList();

            ViewBag.SonHaberler = sonHaberler ?? new List<Haber>();

           
            var ilkHaberler = _haberRepo.GetAll(x => x.Kategori)
                                        .OrderByDescending(h => h.Tarih)
                                        .Take(5)
                                        .ToList();

           
            ViewBag.ToplamHaberSayisi = _haberRepo.GetAll().Count();

            return View(ilkHaberler);
        }

        
        [HttpGet]
        public IActionResult HaberleriGetir(int skip)
        {
           
            var haberler = _haberRepo.GetAll(x => x.Kategori)
                                     .OrderByDescending(x => x.Tarih)
                                     .Skip(skip) 
                                     .Take(5)   
                                     .Select(x => new {
                                         x.Id,
                                         x.Baslik,
                                         x.ResimYolu,
                                         x.OkunmaSayisi,
                                         Tarih = x.Tarih.ToString("dd.MM.yyyy"), 
                                         KategoriAd = x.Kategori.Ad
                                     })
                                     .ToList();

            return Json(haberler);
        }

        
        public IActionResult Detay(int id)
        {
           
            var haber = _haberRepo.GetAll(x => x.Kategori).FirstOrDefault(x => x.Id == id);

            if (haber == null) return RedirectToAction("Index");

           
            haber.OkunmaSayisi += 1;
            _haberRepo.Update(haber);

            
            ViewBag.Yorumlar = _yorumRepo.GetAll()
                                         .Where(y => y.HaberId == id)
                                         .OrderByDescending(y => y.Tarih)
                                         .ToList();

          
            ViewBag.Populer = _haberRepo.GetAll()
                                        .OrderByDescending(x => x.OkunmaSayisi)
                                        .Take(5)
                                        .ToList();

            return View(haber);
        }

       
        [HttpPost]
        public IActionResult YorumYap(Yorum yorum)
        {
            yorum.Tarih = System.DateTime.Now;
            _yorumRepo.Add(yorum);
            return RedirectToAction("Detay", new { id = yorum.HaberId });
        }

    
        public IActionResult KategoriDetay(int id)
        {
            var haberler = _haberRepo.GetAll()
                                     .Where(x => x.KategoriId == id)
                                     .OrderByDescending(x => x.Tarih)
                                     .ToList();

            var kategori = _kategoriRepo.GetById(id);
            ViewBag.KategoriAdi = kategori != null ? kategori.Ad : "Haberler";

            return View(haberler);
        }

        public IActionResult Iletisim()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}