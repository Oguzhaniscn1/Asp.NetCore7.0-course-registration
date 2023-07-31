using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursKayitController:Controller
    {
        private readonly DataContext _context;

        public KursKayitController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurskayitlari = await _context.KursKayitları.Include(x=>x.Ogrenci).Include(x=>x.Kurs).ToListAsync();
            return View(kurskayitlari);
        }



        public async Task<IActionResult> Create()
        {
            ViewBag.Ogrenciler = new SelectList(await _context.Ogrenciler.ToListAsync(), "OfrenciId", "AdSoyad");
            ViewBag.Kurslar = new SelectList(await _context.Kurslar.ToListAsync(), "KursId", "Baslık");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursKayit model)
        {
            model.KayitTarihi = DateTime.Now;
            _context.KursKayitları.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
















    }
}
