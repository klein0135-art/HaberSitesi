using HaberSitesi.Models;
using HaberSitesi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HaberSitesi.Controllers
{
    public class GuvenlikController : Controller
    {
        private readonly IRepository<Uye> _uyeRepo;

        public GuvenlikController(IRepository<Uye> uyeRepo)
        {
            _uyeRepo = uyeRepo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Uye model)
        {
            var uye = _uyeRepo.GetAll().FirstOrDefault(x => x.Eposta == model.Eposta);

            if (uye != null && BCrypt.Net.BCrypt.Verify(model.Sifre, uye.Sifre))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, uye.AdSoyad ?? ""),
                    new Claim(ClaimTypes.Email, uye.Eposta ?? ""),
                    new Claim(ClaimTypes.Role, uye.Rol ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                if (uye.Rol == "Admin") return RedirectToAction("Index", "Haber");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hata = "Eposta veya şifre hatalı";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Uye model)
        {
            var kayitVarMi = _uyeRepo.GetAll().Any(x => x.Eposta == model.Eposta);
            if (kayitVarMi)
            {
                ViewBag.Hata = "Bu Eposta adresi zaten kayıtlı";
                return View(model);
            }

            model.Sifre = BCrypt.Net.BCrypt.HashPassword(model.Sifre);
            model.Rol = "Uye";

            _uyeRepo.Add(model);
            return RedirectToAction("Login");
        }
    }
}