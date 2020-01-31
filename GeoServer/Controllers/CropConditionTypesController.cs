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
    public class CropConditionTypesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: CropConditionTypes
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Index()
        {
            return View(await db.CropConditionTypes.ToListAsync());
        }

        // GET: CropConditionTypes/Details/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropConditionType cropConditionType = await db.CropConditionTypes.FindAsync(id);
            if (cropConditionType == null)
            {
                return HttpNotFound();
            }
            return View(cropConditionType);
        }

        // GET: CropConditionTypes/Create
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CropConditionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Max,Name")] CropConditionType cropConditionType)
        {
            if (ModelState.IsValid)
            {
                db.CropConditionTypes.Add(cropConditionType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cropConditionType);
        }

        // GET: CropConditionTypes/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropConditionType cropConditionType = await db.CropConditionTypes.FindAsync(id);
            if (cropConditionType == null)
            {
                return HttpNotFound();
            }
            return View(cropConditionType);
        }

        // POST: CropConditionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Max,Name")] CropConditionType cropConditionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cropConditionType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cropConditionType);
        }

        // GET: CropConditionTypes/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropConditionType cropConditionType = await db.CropConditionTypes.FindAsync(id);
            if (cropConditionType == null)
            {
                return HttpNotFound();
            }
            return View(cropConditionType);
        }

        // POST: CropConditionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CropConditionType cropConditionType = await db.CropConditionTypes.FindAsync(id);
            db.CropConditionTypes.Remove(cropConditionType);
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
