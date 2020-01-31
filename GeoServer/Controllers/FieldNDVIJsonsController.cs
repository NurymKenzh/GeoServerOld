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
    public class FieldNDVIJsonsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: FieldNDVIJsons
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var fieldNDVIJsons = db.FieldNDVIJsons.Include(f => f.CATO);
            return View(await fieldNDVIJsons.ToListAsync());
        }

        // GET: FieldNDVIJsons/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldNDVIJson fieldNDVIJson = await db.FieldNDVIJsons.FindAsync(id);
            if (fieldNDVIJson == null)
            {
                return HttpNotFound();
            }
            return View(fieldNDVIJson);
        }

        // GET: FieldNDVIJsons/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB");
            return View();
        }

        // POST: FieldNDVIJsons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,catoid,Json")] FieldNDVIJson fieldNDVIJson)
        {
            if (ModelState.IsValid)
            {
                db.FieldNDVIJsons.Add(fieldNDVIJson);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", fieldNDVIJson.catoid);
            return View(fieldNDVIJson);
        }

        // GET: FieldNDVIJsons/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldNDVIJson fieldNDVIJson = await db.FieldNDVIJsons.FindAsync(id);
            if (fieldNDVIJson == null)
            {
                return HttpNotFound();
            }
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", fieldNDVIJson.catoid);
            return View(fieldNDVIJson);
        }

        // POST: FieldNDVIJsons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,catoid,Json")] FieldNDVIJson fieldNDVIJson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fieldNDVIJson).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", fieldNDVIJson.catoid);
            return View(fieldNDVIJson);
        }

        // GET: FieldNDVIJsons/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldNDVIJson fieldNDVIJson = await db.FieldNDVIJsons.FindAsync(id);
            if (fieldNDVIJson == null)
            {
                return HttpNotFound();
            }
            return View(fieldNDVIJson);
        }

        // POST: FieldNDVIJsons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FieldNDVIJson fieldNDVIJson = await db.FieldNDVIJsons.FindAsync(id);
            db.FieldNDVIJsons.Remove(fieldNDVIJson);
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
