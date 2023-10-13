using Microsoft.AspNetCore.Mvc;
using Nazobook.Data;

namespace Nazobook.Controllers;

public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    public IActionResult Banla(int? id)
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

        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);     

        if(kullanici != null)
        {
            if(kullanici.isAdmin)
            {
                userr.isBanned = true;

                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            
        }

        return RedirectToAction("Index", "Home");

        
    }

    public IActionResult Banac(int? id)
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

        int sayi = 0;
        if(HttpContext.Session.GetString("id") != null)
        {
            sayi = int.Parse(HttpContext.Session.GetString("id"));
        }

        User kullanici = _context.User.FirstOrDefault(i=> i.UserId == sayi);

        if(kullanici != null)
        {
            if(kullanici.isAdmin)
            {
                userr.isBanned = false;

                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            
        }

        return RedirectToAction("Index", "Home");

        
    }
}