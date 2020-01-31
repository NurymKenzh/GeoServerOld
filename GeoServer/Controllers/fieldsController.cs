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
using PagedList;

namespace GeoServer.Controllers
{
    public class fieldsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: fields
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(int? Page)
        {
            int PageSize = 100;
            int PageNumber = (Page ?? 1);
            return View(db.fields.Include(f => f.CATO).OrderBy(f => f.gid).ToPagedList(PageNumber, PageSize));
            //return View(await db.fields.ToListAsync());
        }

        // GET: fields/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            field field = await db.fields.FindAsync(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // GET: fields/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: fields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "gid,idfrommap,adm2")] field field)
        {
            if (ModelState.IsValid)
            {
                db.fields.Add(field);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(field);
        }

        // GET: fields/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            field field = await db.fields.FindAsync(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // POST: fields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "gid,idfrommap,adm2")] field field)
        {
            if (ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(field);
        }

        // GET: fields/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            field field = await db.fields.FindAsync(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // POST: fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            field field = await db.fields.FindAsync(id);
            db.fields.Remove(field);
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
            //foreach (field field in db.fields)
            //{
            //    string  AB = field.adm2.ToString().Substring(0, 2),
            //            CD = field.adm2.ToString().Substring(2, 2);
            //    int catoid = catoes
            //        .Where(ca => ca.AB == AB && ca.CD == CD && ca.EF == "00" && ca.HIJ == "000")
            //        .Select(ca => ca.Id)
            //        .FirstOrDefault();
            //    field.catoid = catoid;
            //    db.Entry(field).State = EntityState.Modified;
            //}
            //db.SaveChanges();

            //foreach (field field in db.fields)
            //{
            //    {
            //        field.gid = field.idfrommap;
            //        db.Entry(field).State = EntityState.Modified;
                    
            //        //try
            //        //{
            //        //    db.SaveChanges();
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    string s = field.idfrommap.ToString();
            //        //}
            //    }
                
            //}
            //db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
