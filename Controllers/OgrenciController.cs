using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace efcoreApp.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly DataContext _context;

        public OgrenciController(DataContext context)
        {

            _context = context;

        }


        public async Task<IActionResult> Index()
        {
            //var ogrenciler= await _context.Ogrenciler.ToListAsync();

            return View(await _context.Ogrenciler.ToListAsync());
        }












        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model)
        {


            _context.Ogrenciler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ogr = await _context.Ogrenciler.FindAsync(id);
            var ogr =await _context.Ogrenciler
                .Include(x=>x.KursKayitlari)
                .ThenInclude(x=>x.Kurs)
                .FirstOrDefaultAsync(o => o.OfrenciId == id);

            if (ogr == null)
            {
                return NotFound();
            }

            return View(ogr);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogrenci model)
        {

            if (id != model.OfrenciId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Ogrenciler.Any(o => o.OfrenciId == model.OfrenciId))
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

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogre = await _context.Ogrenciler.FindAsync(id);

            if (ogre == null)
            {
                return NotFound();
            }

            return View(ogre);

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var ogren = await _context.Ogrenciler.FindAsync(id);
            if (ogren == null)
            {
                return NotFound();
            }
            _context.Ogrenciler.Remove(ogren);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");




        }
    }
}

