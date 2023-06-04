using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VFT.OCT.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFT.OCT.Controllers
{
    public class VFTController : Controller
    {
        private readonly VFT_OCTContext _context;
        SqlCommand Command { get; set; }
        SqlConnection Connection { get; set; }

        public VFTController(VFT_OCTContext context)
        {
            _context = context;
            string connectionString = "server=(localdb)\\MSSQLLocalDB; database=VFT_OCT; Trusted_Connection=True";
            Connection = new SqlConnection(connectionString);
        }

        // GET: VFT
        public async Task<IActionResult> Index()
        {
              return _context.Vfts != null ? 
                          View(await _context.Vfts.ToListAsync()) :
                          Problem("Entity set 'VFT_OCTContext.Vfts'  is null.");
        }

        // GET: VFT/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Vfts == null)
            {
                return NotFound();
            }

            var vft = await _context.Vfts
                .FirstOrDefaultAsync(m => m.ScanId == id);
            if (vft == null)
            {
                return NotFound();
            }

            return View(vft);
        }

        // GET: VFT/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VFT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScanId,PatientName,ReferredDrName,ReFfacility,Date")] Vft vft)
        {
            if (ModelState.IsValid)
            {
                if (_context.Vfts.Find(vft.ScanId) != null)
                {
                    ViewBag.tryAgain = "Scan Id Already Used!!";
                    
                    return View();
                }
                _context.Add(vft);
                await _context.SaveChangesAsync();
                //ViewBag.success = "Patient Added Successfully";
                return View("Success");
            }
            return RedirectToAction("Create");
        }

              // GET: VFT/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var id1=id.Replace("%2F", "/");
            if (id1 == null || _context.Vfts == null)
            {
                return NotFound();
            }

            var vft = await _context.Vfts.FindAsync(id1);
            if (vft == null)
            {
                return NotFound();
            }
            return View(vft);
        }
        [HttpPost]
        public ViewResult DailyRecords(DateTime today)
        {
            List<Vft> vft = null;

            vft=_context.Vfts.Where(x => x.Date == today).ToList();
            //return _context.Vfts.Where(x => x.Date >= today.Date && x.Date <= today.Date) != null ?
            //             View(await _context.Vfts.Where(x => x.Date >= today.Date && x.Date <= today.Date).ToListAsync()) :
            //             Problem("Entity set 'VFT_OCTContext.Vfts'  is null.");
            ////return await _context.Vfts.Where(x => x.Date == today.Date);
            ViewBag.today=today.ToString("dddd, dd MMMM yyyy");
            
            return View(vft);
        }

        [HttpPost]
        public ViewResult ViewReportVFT(DateTime stDate, DateTime eDate)
        {

            //SqlDataReader rdr=null;
            //DataTable dt= new DataTable();
            List<VFTreport> vftreport = new List<VFTreport>();



            vftreport=_context.VFTreports.FromSqlInterpolated($"select * from udf_GenerateReportByDate({stDate}, {eDate})").OrderByDescending(x => x.Amount).ToList();
            //List<VFTreport> res = new List<VFTreport>();
            // var d = (from c in _context.Vfts
            //            where c.Date >= stDate && c.Date <= eDate
            //            group c by new { c.ReferredDrName, c.ReFfacility } into g
            //            select new
            //            {
            //                ReferredDoctor=g.Key.ReferredDrName,
            //                ReferredFacility=g.Key.ReFfacility,
            //                NumberOfReferrals=g.Count(),
            //                Amount=g.Count()*10

            //            });

            //      foreach(var g in d )
            //{
            //    res.Add(new VFTreport()
            //    {
            //        ReferredDoctor = g.ReferredDoctor,
            //        ReferredFacility = g.ReferredFacility,
            //        NumberOfReferrals = g.NumberOfReferrals,
            //        Amount = g.Amount
            //    });
            //}
            // return View(res);
            return View(vftreport);

            //1................

            //var stDate1=Convert.ToDateTime(stDate);
            //var eDate1=Convert.ToDateTime(eDate);

            //Command = new SqlCommand("select * from udf_fetchRecordsByDate(@st,@end)", Connection);
            //Command.Parameters.AddWithValue("@st", stDate1);
            //Command.Parameters.AddWithValue("@end", eDate1);
            //Connection.Open();
            //rdr=Command.ExecuteReader(CommandBehavior.CloseConnection);
            //while (rdr.Read()) 
            //{
            //    vft.Add(
            //                new Vft()
            //                {
            //                    ScanId = rdr[0].ToString(),
            //                    PatientName = rdr[1].ToString(),
            //                    ReferredDrName = rdr[2].ToString(),
            //                    ReFfacility = rdr[3].ToString(),
            //                    Date = Convert.ToDateTime(rdr[4])
            //                }
            //            );

            //}

            //2................

            //using (SqlDataAdapter ad=new SqlDataAdapter(Command))
            //{
            //    ad.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            vft.Add(
            //                new Vft() {
            //                    ScanId = dr[0].ToString(),
            //                    PatientName = dr[1].ToString(),
            //            ReferredDrName = dr[2].ToString(),
            //            ReFfacility = dr[3].ToString(),
            //            Date = Convert.ToDateTime(dr[4])
            //        }
            //            );
            //        }
            //    }
            //    vft = null;
            //}
            // List<Vft> vf = null;
            // if (stDate > eDate) { return null; }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ScanId,PatientName,ReferredDrName,ReFfacility,Date")] Vft vft)
        {
            var id1 = id.Replace("%2F", "/");
            if (id1 != vft.ScanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vft);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VftExists(vft.ScanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.id = vft.ScanId;
                return View("EditSuccess");
            }
            return View(vft);
        }

        // GET: VFT/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Vfts == null)
            {
                return NotFound();
            }

            var vft = await _context.Vfts
                .FirstOrDefaultAsync(m => m.ScanId == id);
            if (vft == null)
            {
                return NotFound();
            }

            return View(vft);
        }

        // POST: VFT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Vfts == null)
            {
                return Problem("Entity set 'VFT_OCTContext.Vfts'  is null.");
            }
            var vft = await _context.Vfts.FindAsync(id);
            if (vft != null)
            {
                _context.Vfts.Remove(vft);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VftExists(string id)
        {
          return (_context.Vfts?.Any(e => e.ScanId == id)).GetValueOrDefault();
        }
    }
}
