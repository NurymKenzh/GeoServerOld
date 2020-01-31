using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GeoServer.Models;

namespace GeoServer.Controllers
{
    public class admpol1Controller : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: admpol1
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Admpol1.Include(a => a.CATO).ToListAsync());
        }

        // GET: admpol1/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admpol1 admpol1 = await db.Admpol1.FindAsync(id);
            if (admpol1 == null)
            {
                return HttpNotFound();
            }
            return View(admpol1);
        }

        // GET: admpol1/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: admpol1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "gid,code_obl")] admpol1 admpol1)
        {
            if (ModelState.IsValid)
            {
                db.Admpol1.Add(admpol1);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(admpol1);
        }

        // GET: admpol1/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admpol1 admpol1 = await db.Admpol1.FindAsync(id);
            if (admpol1 == null)
            {
                return HttpNotFound();
            }
            return View(admpol1);
        }

        // POST: admpol1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "gid,code_obl")] admpol1 admpol1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admpol1).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(admpol1);
        }

        // GET: admpol1/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admpol1 admpol1 = await db.Admpol1.FindAsync(id);
            if (admpol1 == null)
            {
                return HttpNotFound();
            }
            return View(admpol1);
        }

        // POST: admpol1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            admpol1 admpol1 = await db.Admpol1.FindAsync(id);
            db.Admpol1.Remove(admpol1);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Update()
        {
            //var catoes = db.CATOes.ToList();
            //foreach (admpol1 admpol1 in db.Admpol1)
            //{
            //    string AB = admpol1.code_obl.ToString().Substring(0, 2);
            //    //CD = admpol1.code_obl.ToString().Substring(2, 2);
            //    int catoid = catoes
            //        .Where(ca => ca.AB == AB && ca.CD == "00" && ca.EF == "00" && ca.HIJ == "000")
            //        .Select(ca => ca.Id)
            //        .FirstOrDefault();
            //    admpol1.catoid = catoid;
            //    db.Entry(admpol1).State = EntityState.Modified;

            //}
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
