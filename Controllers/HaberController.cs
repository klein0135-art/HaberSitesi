using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using HaberSitesi.Models;
using HaberSitesi.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace HaberSitesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HaberController : Controller
    {
        private readonly IRepository<Haber> _haberRepo;
        private readonly IRepository<Kategori> _kategoriRepo;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HaberController(IRepository<Haber> haberRepo, IRepository<Kategori> kategoriRepo, IWebHostEnvironment hostEnvironment)
        {
            _haberRepo = haberRepo;
            _kategoriRepo = kategoriRepo;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var haberler = _haberRepo.GetAll(x => x.Kategori);
            return View(haberler);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var haber = _haberRepo.GetAll(x => x.Kategori).FirstOrDefault(m => m.Id == id);

            if (haber == null) return NotFound();

            return View(haber);
        }

        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_kategoriRepo.GetAll(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Haber haber)
        {
            if (ModelState.IsValid)
            {
                if (haber.ResimDosyasi != null)
                {
                    var eklenti = Path.GetExtension(haber.ResimDosyasi.FileName).ToLower();
                    if (eklenti != ".jpg" && eklenti != ".jpeg" && eklenti != ".png")
                    {
                        ModelState.AddModelError("ResimDosyasi", "Sadece JPG, JPEG veya PNG yükleyebilirsiniz.");
                        ViewData["KategoriId"] = new SelectList(_kategoriRepo.GetAll(), "Id", "Ad", haber.KategoriId);
                        return View(haber);
                    }

                    if (haber.ResimDosyasi.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ResimDosyasi", "Resim boyutu 2 MB'dan büyük olamaz.");
                        ViewData["KategoriId"] = new SelectList(_kategoriRepo.GetAll(), "Id", "Ad", haber.KategoriId);
                        return View(haber);
                    }

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string dosyaAdi = Path.GetFileNameWithoutExtension(haber.ResimDosyasi.FileName);
                    string yeniDosyaAdi = dosyaAdi + DateTime.Now.ToString("yymmssfff") + eklenti;
                    string yol = Path.Combine(wwwRootPath, "img", yeniDosyaAdi);

                    using (var fileStream = new FileStream(yol, FileMode.Create))
                    {
                        await haber.ResimDosyasi.CopyToAsync(fileStream);
                    }

                    haber.ResimYolu = yeniDosyaAdi;
                }
                else
                {
                    haber.ResimYolu = "no-image.jpg";
                }

                _haberRepo.Add(haber);
                return RedirectToAction(nameof(Index));
            }

            ViewData["KategoriId"] = new SelectList(_kategoriRepo.GetAll(), "Id", "Ad", haber.KategoriId);
            return View(haber);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var haber = _haberRepo.GetById(id.Value);
            if (haber == null) return NotFound();

            ViewData["KategoriId"] = new SelectList(_kategoriRepo.GetAll(), "Id", "Ad", haber.KategoriId);
            return View(haber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Haber haber)
        {
            if (id != haber.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (haber.ResimDosyasi != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string dosyaAdi = Path.GetFileNameWithoutExtension(haber.ResimDosyasi.FileName);
                        string eklenti = Path.GetExtension(haber.ResimDosyasi.FileName);
                        string yeniDosyaAdi = dosyaAdi + DateTime.Now.ToString("yymmssfff") + eklenti;
                        string yol = Path.Combine(wwwRootPath, "img", yeniDosyaAdi);

                        using (var fileStream = new FileStream(yol, FileMode.Create))
                        {
                            await haber.ResimDosyasi.CopyToAsync(fileStream);
                        }

                        haber.ResimYolu = yeniDosyaAdi;
                    }
                    else
                    {
                        var eski = _haberRepo.GetById(id);
                        if (eski != null)
                        {
                            haber.ResimYolu = eski.ResimYolu;
                        }
                    }

                    _haberRepo.Update(haber);
                }
                catch (Exception)
                {
                    if (_haberRepo.GetById(id) == null) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KategoriId"] = new SelectList(_kategoriRepo.GetAll(), "Id", "Ad", haber.KategoriId);
            return View(haber);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var haber = _haberRepo.GetAll(x => x.Kategori).FirstOrDefault(m => m.Id == id);
            if (haber == null) return NotFound();

            return View(haber);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _haberRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}