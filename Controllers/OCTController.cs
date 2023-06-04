using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VFT.OCT.Models;

namespace VFT.OCT.Controllers
{
    public class OCTController : Controller
    {
        private readonly VFT_OCTContext _context;

        public OCTController(VFT_OCTContext context)
        {
            _context = context;
        }

        // GET: OCT
        public async Task<IActionResult> Index()
        {
              return _context.Octs != null ? 
                          View(await _context.Octs.ToListAsync()) :
                          Problem("Entity set 'VFT_OCTContext.Octs'  is null.");
        }

        // GET: OCT/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Octs == null)
            {
                return NotFound();
            }

            var oct = await _context.Octs
                .FirstOrDefaultAsync(m => m.ScanId == id);
            if (oct == null)
            {
                return NotFound();
            }

            return View(oct);
        }

        // GET: OCT/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OCT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScanId,PatientName,ReferredDrName,ReFfacility,Onh,Macula,Pachymetry,Date")] Oct oct)
        {
            if (ModelState.IsValid)
            {
                if (_context.Vfts.Find(oct.ScanId) != null)
                {
                    ViewBag.tryAgain = "Scan Id Already Used!!";
                    return View();
                }
                    _context.Add(oct);
                await _context.SaveChangesAsync();
                
                return View("OctRegistrySuccess");
            }
            return View(oct);
        }

        // GET: OCT/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var id1 = id.Replace("%2F", "/");
            if (id1 == null || _context.Octs == null)
            {
                return NotFound();
            }

            var oct = await _context.Octs.FindAsync(id1);
            if (oct == null)
            {
                return NotFound();
            }
            return View(oct);
        }

        // POST: OCT/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ScanId,PatientName,ReferredDrName,ReFfacility,Onh,Macula,Pachymetry,Date")] Oct oct)
        {
            var id1 = id.Replace("%2F", "/");
            if (id1 != oct.ScanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OctExists(oct.ScanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.id = oct.ScanId;
                return View("EditSuccess");
            }
            return View(oct);
        }
        [HttpPost]
        public ViewResult DailyRecords(DateTime today)
        {
            List<Oct> oct = null;

            oct = _context.Octs.Where(x => x.Date == today).ToList();
            ViewBag.today = today.ToString("dddd, dd MMMM yyyy");

            return View(oct);
        }

        [HttpPost]
        public ViewResult Search(DateTime stDate, DateTime eDate)
        {

            List<Oct> oct = new List<Oct>();
            try
            {
                oct = _context.Octs.Where(x => x.Date >= stDate.Date && x.Date <= eDate.Date).ToList();

              
            }
            catch (Exception ex)
            {
                oct = null;
            }

            return View(oct);
        }
        public ViewResult ViewReportOCT(DateTime stDate, DateTime eDate)
        {

            List<OCTreport> octreport = new List<OCTreport>();



            octreport = _context.OCTreports.FromSqlInterpolated($"select * from udf_GenerateOCTreport({stDate}, {eDate})").
                OrderByDescending(x=>x.Amount).ToList();
            return View(octreport);
        }
            
           
            //// POST: OCT/Delete/5
            //[HttpPost, ActionName("Delete")]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> DeleteConfirmed(string id)
            //{
            //    if (_context.Octs == null)
            //    {
            //        return Problem("Entity set 'VFT_OCTContext.Octs'  is null.");
            //    }
            //    var oct = await _context.Octs.FindAsync(id);
            //    if (oct != null)
            //    {
            //        _context.Octs.Remove(oct);
            //    }

            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            private bool OctExists(string id)
        {
          return (_context.Octs?.Any(e => e.ScanId == id)).GetValueOrDefault();
        }
    }
}
