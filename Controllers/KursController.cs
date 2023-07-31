using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController : Controller
    {

        private readonly DataContext _context;

        public KursController(DataContext context) 
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurslar=await _context.Kurslar.Include(k=>k.Ogretmen).ToListAsync();
            return View(kurslar);
        }





        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId", "AdSoyad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursViewModel model)
        {

            if (ModelState.IsValid) {
                _context.Kurslar.Add(new Kurs
                {
                    KursId = model.KursId,
                    Baslık = model.Baslık,
                    OgretmenId = model.OgretmenId,
                    
                });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");


            return View(model);


        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context
                .Kurslar.Include(x => x.KursKayitlari)
                .ThenInclude(x => x.Ogrenci)
                .Select(k=>new KursViewModel
                {
                    KursId=k.KursId,
                    Baslık=k.Baslık,
                    OgretmenId=k.OgretmenId,
                    KursKayitlari=k.KursKayitlari
                })
                .FirstOrDefaultAsync(k=>k.KursId==id);

            if (kurs == null)
            {
                return NotFound();
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");


            return View(kurs);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel model)
        {

            if (id != model.KursId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Kurs() {KursId=model.KursId,Baslık=model.Baslık,OgretmenId=model.OgretmenId});
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!_context.Kurslar.Any(o => o.KursId == model.KursId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurslar.FindAsync(id);

            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);

        }



        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var kursla = await _context.Kurslar.FindAsync(id);
            if (kursla == null)
            {
                return NotFound();
            }
            _context.Kurslar.Remove(kursla);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");




        }


    }
}
