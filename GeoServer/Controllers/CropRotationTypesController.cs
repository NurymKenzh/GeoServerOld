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
    public class CropRotationTypesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: CropRotationTypes
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Index()
        {
            return View(await db.CropRotationTypes.ToListAsync());
        }

        // GET: CropRotationTypes/Details/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropRotationType cropRotationType = await db.CropRotationTypes.FindAsync(id);
            if (cropRotationType == null)
            {
                return HttpNotFound();
            }
            return View(cropRotationType);
        }

        // GET: CropRotationTypes/Create
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CropRotationTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,NameRU,NameKK,NameEN")] CropRotationType cropRotationType)
        {
            if (ModelState.IsValid)
            {
                db.CropRotationTypes.Add(cropRotationType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cropRotationType);
        }

        // GET: CropRotationTypes/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropRotationType cropRotationType = await db.CropRotationTypes.FindAsync(id);
            if (cropRotationType == null)
            {
                return HttpNotFound();
            }
            return View(cropRotationType);
        }

        // POST: CropRotationTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NameRU,NameKK,NameEN")] CropRotationType cropRotationType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cropRotationType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cropRotationType);
        }

        // GET: CropRotationTypes/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropRotationType cropRotationType = await db.CropRotationTypes.FindAsync(id);
            if (cropRotationType == null)
            {
                return HttpNotFound();
            }
            return View(cropRotationType);
        }

        // POST: CropRotationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CropRotationType cropRotationType = await db.CropRotationTypes.FindAsync(id);
            db.CropRotationTypes.Remove(cropRotationType);
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
