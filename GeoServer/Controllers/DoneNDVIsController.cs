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
    public class DoneNDVIsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: DoneNDVIs
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var doneNDVIs = db.DoneNDVIs.Include(d => d.CATO);
            return View(await doneNDVIs.ToListAsync());
        }

        // GET: DoneNDVIs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoneNDVI doneNDVI = await db.DoneNDVIs.FindAsync(id);
            if (doneNDVI == null)
            {
                return HttpNotFound();
            }
            return View(doneNDVI);
        }

        // GET: DoneNDVIs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB");
            return View();
        }

        // POST: DoneNDVIs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,catoid,MeanAverage")] DoneNDVI doneNDVI)
        {
            if (ModelState.IsValid)
            {
                db.DoneNDVIs.Add(doneNDVI);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", doneNDVI.catoid);
            return View(doneNDVI);
        }

        // GET: DoneNDVIs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoneNDVI doneNDVI = await db.DoneNDVIs.FindAsync(id);
            if (doneNDVI == null)
            {
                return HttpNotFound();
            }
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", doneNDVI.catoid);
            return View(doneNDVI);
        }

        // POST: DoneNDVIs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,catoid,MeanAverage")] DoneNDVI doneNDVI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doneNDVI).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", doneNDVI.catoid);
            return View(doneNDVI);
        }

        // GET: DoneNDVIs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoneNDVI doneNDVI = await db.DoneNDVIs.FindAsync(id);
            if (doneNDVI == null)
            {
                return HttpNotFound();
            }
            return View(doneNDVI);
        }

        // POST: DoneNDVIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DoneNDVI doneNDVI = await db.DoneNDVIs.FindAsync(id);
            db.DoneNDVIs.Remove(doneNDVI);
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
    }
}
