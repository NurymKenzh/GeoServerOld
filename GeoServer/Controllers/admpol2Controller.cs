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
    public class admpol2Controller : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: admpol2
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Admpol2.Include(a => a.CATO).ToListAsync());
        }

        // GET: admpol2/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admpol2 admpol2 = await db.Admpol2.FindAsync(id);
            if (admpol2 == null)
            {
                return HttpNotFound();
            }
            return View(admpol2);
        }

        // GET: admpol2/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: admpol2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "gid,kato")] admpol2 admpol2)
        {
            if (ModelState.IsValid)
            {
                db.Admpol2.Add(admpol2);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(admpol2);
        }

        // GET: admpol2/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admpol2 admpol2 = await db.Admpol2.FindAsync(id);
            if (admpol2 == null)
            {
                return HttpNotFound();
            }
            return View(admpol2);
        }

        // POST: admpol2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "gid,kato")] admpol2 admpol2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admpol2).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(admpol2);
        }

        // GET: admpol2/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admpol2 admpol2 = await db.Admpol2.FindAsync(id);
            if (admpol2 == null)
            {
                return HttpNotFound();
            }
            return View(admpol2);
        }

        // POST: admpol2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            admpol2 admpol2 = await db.Admpol2.FindAsync(id);
            db.Admpol2.Remove(admpol2);
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
            //foreach (admpol2 admpol2 in db.Admpol2)
            //{
            //    string AB = admpol2.kato.ToString().Substring(0, 2),
            //            CD = admpol2.kato.ToString().Substring(2, 2);
            //    int catoid = catoes
            //        .Where(ca => ca.AB == AB && ca.CD == CD && ca.EF == "00" && ca.HIJ == "000")
            //        .Select(ca => ca.Id)
            //        .FirstOrDefault();
            //    admpol2.catoid = catoid;
            //    db.Entry(admpol2).State = EntityState.Modified;
            //}
            //db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
