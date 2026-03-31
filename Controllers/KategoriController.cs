using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HaberSitesi.Models;
using HaberSitesi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HaberSitesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KategoriController : Controller
    {
        private readonly IRepository<Kategori> _repo;

        public KategoriController(IRepository<Kategori> repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var kategoriler = _repo.GetAll();
            return View(kategoriler);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var kategori = _repo.GetById(id.Value);
            if (kategori == null) return NotFound();
            return View(kategori);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Ad")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(kategori);
                return RedirectToAction(nameof(Index));
            }

            return View(kategori);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var kategori = _repo.GetById(id.Value);
            if (kategori == null) return NotFound();
            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Ad")] Kategori kategori)
        {
            if (id != kategori.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _repo.Update(kategori);
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var kategori = _repo.GetById(id.Value);
            if (kategori == null) return NotFound();

            return View(kategori);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}