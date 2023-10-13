using Microsoft.AspNetCore.Mvc;
using Nazobook.Data;
using Nazobook.Models;

namespace Nazobook.Controllers
{
    public class ProfilController : Controller
    {

        private readonly DataContext _context;

        public ProfilController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Profilim");
        }

        public IActionResult Profilim()
        {
            int sayi = 0;
            if(HttpContext.Session.GetString("id") != null)
            {
                sayi = int.Parse(HttpContext.Session.GetString("id"));
            }

            User userr = _context.User.FirstOrDefault(i=> i.UserId == sayi);

            if(userr == null)
            {
                return RedirectToAction("Index","Home");
            }

            userr.Gonderiler = _context.Gonderi.Where(i=> i.UserId == sayi).OrderByDescending(i=> i.GonderiId).ToList();

            return View(userr);
        }

        public IActionResult Ayarlar()
        {
            ViewData["girisyapildi"] = "0";
            ViewData["kullaniciad"] = "";
            ViewData["resimurl"] = "";

            int sayi = 0;
            if(HttpContext.Session.GetString("id") != null)
            {
                sayi = int.Parse(HttpContext.Session.GetString("id"));
            }

            User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);     


            if(kullanici != null)
            {
                ViewData["girisyapildi"] = "1";
                ViewData["kullaniciad"] = kullanici.KullaniciAdi;
                if(kullanici.ResimYol == "" || kullanici.ResimYol == null)
                {
                    ViewData["resimurl"] = "/img/bospp.jpg";  
                }else
                {
                    ViewData["resimurl"] = "/kprofil/"+@kullanici.UserId+"/"+@kullanici.ResimYol;
                }

            }
            if(kullanici == null)
            {
                return RedirectToAction("Index","Home");
            }
            
            User userr = _context.User.FirstOrDefault(i=> i.UserId == sayi);

            UserModel model = new UserModel() {
                UserId = userr.UserId,
                KullaniciAdi = userr.KullaniciAdi,
                Eposta = userr.Eposta
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Ayarlar(UserModel user, string? sifreonayla, IFormFile? profilfoto, string? yenisifre) 
        {
            int sayi = 0;
            if(HttpContext.Session.GetString("id") != null)
            {
                sayi = int.Parse(HttpContext.Session.GetString("id"));
            }

            User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);    

            ViewData["girisyapildi"] = "0";
            ViewData["kullaniciad"] = "";
            ViewData["resimurl"] = "";


            if(kullanici != null)
            {
                ViewData["girisyapildi"] = "1";
                ViewData["kullaniciad"] = kullanici.KullaniciAdi;
                if(kullanici.ResimYol == "" || kullanici.ResimYol == null)
                {
                    ViewData["resimurl"] = "/img/bospp.jpg";  
                }else
                {
                    ViewData["resimurl"] = "/kprofil/"+@kullanici.UserId+"/"+@kullanici.ResimYol;
                }

            }
            if(kullanici == null)
            {
                return RedirectToAction("Index","Home");
            }
            
            if(ModelState.IsValid)
            {
                User entity = _context.User.FirstOrDefault(i=> i.UserId == kullanici.UserId);
                if(entity == null)
                {
                    return View(user);
                }
                if(entity.Sifre != sifreonayla)
                {
                    
                    return View(user);
                }
                entity.KullaniciAdi = user.KullaniciAdi.Trim();
                entity.Eposta = user.Eposta;
                if(yenisifre == "" || yenisifre == null)
                {
                    
                }else
                {
                    entity.Sifre = yenisifre;
                }
                if(profilfoto != null)
                {
                    var extention = Path.GetExtension(profilfoto.FileName);
                    string fileName = "1"+extention;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\kprofil\\{entity.UserId}", fileName);
                    entity.ResimYol = fileName;

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await profilfoto.CopyToAsync(stream);
                    }
                }

                _context.SaveChanges();

                return RedirectToAction("Profilim");
                
            }

            return View(user);
        }



        public IActionResult Profiller(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            User userr = _context.User.FirstOrDefault(i=> i.UserId == id);

            if(userr == null)
            {
                return RedirectToAction("Index", "Home");
            }

            userr.Gonderiler = _context.Gonderi.Where(i=> i.UserId == id).OrderByDescending(i=> i.GonderiId).ToList();

            ViewData["girisyapildi"] = "0";
            ViewData["kullaniciad"] = "";
            ViewData["resimurl"] = "";
            ViewData["admin"] = "0";

            int sayi = 0;
            if(HttpContext.Session.GetString("id") != null)
            {
                sayi = int.Parse(HttpContext.Session.GetString("id"));
            }

            User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);     


            if(kullanici != null)
            {
                ViewData["girisyapildi"] = "1";
                ViewData["kullaniciad"] = kullanici.KullaniciAdi;
                if(kullanici.ResimYol == "" || kullanici.ResimYol == null)
                {
                    ViewData["resimurl"] = "/img/bospp.jpg";  
                }else
                {
                    ViewData["resimurl"] = "/kprofil/"+@kullanici.UserId+"/"+@kullanici.ResimYol;
                }

                if(kullanici.isAdmin)
                {
                    ViewData["admin"] = "1";
                }

            }



            return View(userr);
        }
    }
}