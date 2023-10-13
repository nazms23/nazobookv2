using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nazobook.Data;
using Nazobook.Models;

namespace Nazobook.Controllers;

public class HomeController : Controller
{
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }


    public IActionResult Index()
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);     

        List<Gonderi> gonderiler = _context.Gonderi.OrderByDescending(i=> i.GonderiId).ToList();

        foreach(var gonderli in gonderiler)
        {
            gonderli.User = _context.User.FirstOrDefault(i=> i.UserId == gonderli.UserId);
        }

        HomeIndexModel model = new HomeIndexModel() {
            User = kullanici,
            Gonderi = gonderiler
        };

        return View(model);
    }

    public IActionResult Duyurular()
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);     

        List<Gonderi> gonderiler = _context.Gonderi.Where(i=> i.isDuyuru == true).OrderByDescending(i=> i.GonderiId).ToList();

        foreach(var gonderli in gonderiler)
        {
            gonderli.User = _context.User.FirstOrDefault(i=> i.UserId == gonderli.UserId);
        }

        HomeIndexModel model = new HomeIndexModel() {
            User = kullanici,
            Gonderi = gonderiler
        };


        return View(model);
    }

    public IActionResult Kurallar()
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);    
        return View(kullanici);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> Gonder(GonderiModel model,int? UserId, IFormFile? gonderifoto, int Duyuru)
    {
        model.isDuyuru = false;
        if(Duyuru == 1)
        {
            int sayi = 0;
            if(HttpContext.Session.GetString("id") != null)
            {
                sayi = int.Parse(HttpContext.Session.GetString("id"));
            }

            User kullanici2 = _context.User.FirstOrDefault(i=> i.UserId == sayi); 

            if(kullanici2 != null)
            {
                if(kullanici2.isAdmin)
                {
                    model.isDuyuru = true;
                }
            }

            
        }
        var kullanici = _context.User.FirstOrDefault(i => i.UserId == UserId);

        if(kullanici == null)
        {
            return NotFound();
        }

        if(ModelState.IsValid)
        {
            int gonderino = kullanici.GonderiSayi + 1;
            kullanici.GonderiSayi += 1;
            kullanici.GonderiSayi2 +=1; 

            string ResimYol = "";
            if(gonderifoto != null)
            {
                var path2 = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\kgonderiler\\{kullanici.UserId}", gonderino.ToString());

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                var extention = Path.GetExtension(gonderifoto.FileName);
                string fileName = "1"+extention;
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\kgonderiler\\{kullanici.UserId}\\{gonderino}", fileName);
                ResimYol = fileName;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await gonderifoto.CopyToAsync(stream);
                }
            }

            var baslik1 = "";
            if(model.Baslik != "")
            {
                baslik1 = model.Baslik.Trim();
            }else
            {
                baslik1 = model.Baslik;
            }

            var mesaj1 = "";
            if(model.Mesaj != "")
            {
                mesaj1 = model.Mesaj.Trim();
            }else
            {
                mesaj1 = model.Mesaj;
            }

            Gonderi gonderi = new Gonderi() {
                Baslik = baslik1,
                Mesaj = mesaj1,
                ResimYol = ResimYol,
                isDuyuru = model.isDuyuru,
                User = kullanici,
                UserId = kullanici.UserId,
                GonderiNo = gonderino,
                GonderiTarih = DateTime.Now
            };

            _context.Gonderi.Add(gonderi);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");

    }


    public IActionResult Girisyap()
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi); 

        if(kullanici != null)
        {
            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpPost]
    public IActionResult Girisyap(Usergirismodel model)
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi); 

        if(kullanici != null)
        {
            return RedirectToAction("Index");
        }

        if(ModelState.IsValid)
        {
            var kontrol = _context.User.FirstOrDefault(i=> i.Eposta == model.Eposta && i.Sifre == model.Sifre);

            if(kontrol == null)
            {
                ModelState.AddModelError("Eposta", "E posta veya şifre hatalı");
            }

            if(ModelState.IsValid)
            {
                HttpContext.Session.SetString("id", kontrol.UserId.ToString());
                return RedirectToAction("index");
            }
        }
        return View(model);

        
    }

    public IActionResult Kayitol()
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi); 

        if(kullanici != null)
        {
            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpPost]
    public IActionResult Kayitol(User model)
    {
        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi); 

        if(kullanici != null)
        {
            return RedirectToAction("Index");
        }

        User yenikullanici = model;
        yenikullanici.KullaniciAdi = yenikullanici.KullaniciAdi;
        yenikullanici.isBanned = false;
        yenikullanici.GonderiSayi = 0;
        yenikullanici.GonderiSayi2 = 0;
        yenikullanici.isAdmin = false;
        yenikullanici.Tarih = DateTime.Now;
        yenikullanici.UyePp = 1;
        yenikullanici.ResimYol = null;
        yenikullanici.Gonderiler = null;

        if(ModelState.IsValid)
        {
            yenikullanici.KullaniciAdi = yenikullanici.KullaniciAdi.Trim();
            var kullaniciadisayac = _context.User.Where(i => i.KullaniciAdi.ToLower() == yenikullanici.KullaniciAdi.ToLower()).Count();
            var epostasayac = _context.User.Where(i => i.Eposta.ToLower() == yenikullanici.Eposta.ToLower()).Count();

            if(kullaniciadisayac > 0)
            {
                ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı alınamaz");
            }
            if(epostasayac > 0)
            {
                ModelState.AddModelError("Eposta", "Bu e posta alınamaz");
            }
            if(ModelState.IsValid)
            {
                _context.User.Add(yenikullanici);
                _context.SaveChanges();
                var path2 = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\kgonderiler", yenikullanici.UserId.ToString());

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                var path3 = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\kprofil", yenikullanici.UserId.ToString());

                if (!Directory.Exists(path3))
                {
                    Directory.CreateDirectory(path3);
                }
                return RedirectToAction("Girisyap");
            }
            return View(model);
        }
        return View(model);
    }

    public IActionResult Cikisyap()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }


    public IActionResult Gonderisil(int? id)
    {
        if(id == null)
        {
            return RedirectToAction("Index");
        }

        Gonderi gonderi = _context.Gonderi.FirstOrDefault(i=> i.GonderiId == id);

        if(gonderi == null)
        {
            return RedirectToAction("Index");
        }



        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);     

        if(kullanici != null)
        {
            if(kullanici.UserId == gonderi.UserId)
            {
                _context.Gonderi.Remove(gonderi);
                kullanici.GonderiSayi2 -= 1;
                _context.SaveChanges();
                return RedirectToAction("Profilim", "Profil");
            }
            else if(kullanici.isAdmin)
            {
                User kullanici1 = _context.User.FirstOrDefault(i=> i.UserId == gonderi.UserId);     
                kullanici1.GonderiSayi2 -= 1;
                _context.Gonderi.Remove(gonderi);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        return RedirectToAction("Index");
    }
}
