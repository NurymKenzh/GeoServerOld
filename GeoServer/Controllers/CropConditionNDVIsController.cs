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
    public class CropConditionNDVIsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: CropConditionNDVIs
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var cropConditionNDVIs = db.CropConditionNDVIs.Include(c => c.CATO).Include(c => c.CropConditionType);
            return View(await cropConditionNDVIs.ToListAsync());
        }

        // GET: CropConditionNDVIs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropConditionNDVI cropConditionNDVI = await db.CropConditionNDVIs.FindAsync(id);
            if (cropConditionNDVI == null)
            {
                return HttpNotFound();
            }
            return View(cropConditionNDVI);
        }

        // GET: CropConditionNDVIs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB");
            ViewBag.CropConditionTypeId = new SelectList(db.CropConditionTypes, "Id", "Name");
            return View();
        }

        // POST: CropConditionNDVIs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,catoid,CropConditionTypeId")] CropConditionNDVI cropConditionNDVI)
        {
            if (ModelState.IsValid)
            {
                db.CropConditionNDVIs.Add(cropConditionNDVI);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", cropConditionNDVI.catoid);
            ViewBag.CropConditionTypeId = new SelectList(db.CropConditionTypes, "Id", "Name", cropConditionNDVI.CropConditionTypeId);
            return View(cropConditionNDVI);
        }

        // GET: CropConditionNDVIs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropConditionNDVI cropConditionNDVI = await db.CropConditionNDVIs.FindAsync(id);
            if (cropConditionNDVI == null)
            {
                return HttpNotFound();
            }
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", cropConditionNDVI.catoid);
            ViewBag.CropConditionTypeId = new SelectList(db.CropConditionTypes, "Id", "Name", cropConditionNDVI.CropConditionTypeId);
            return View(cropConditionNDVI);
        }

        // POST: CropConditionNDVIs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,catoid,CropConditionTypeId")] CropConditionNDVI cropConditionNDVI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cropConditionNDVI).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.catoid = new SelectList(db.CATOes, "Id", "AB", cropConditionNDVI.catoid);
            ViewBag.CropConditionTypeId = new SelectList(db.CropConditionTypes, "Id", "Name", cropConditionNDVI.CropConditionTypeId);
            return View(cropConditionNDVI);
        }

        // GET: CropConditionNDVIs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropConditionNDVI cropConditionNDVI = await db.CropConditionNDVIs.FindAsync(id);
            if (cropConditionNDVI == null)
            {
                return HttpNotFound();
            }
            return View(cropConditionNDVI);
        }

        // POST: CropConditionNDVIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CropConditionNDVI cropConditionNDVI = await db.CropConditionNDVIs.FindAsync(id);
            db.CropConditionNDVIs.Remove(cropConditionNDVI);
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
