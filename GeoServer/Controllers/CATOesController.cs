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
using System.IO;
using System.Text;
using PagedList;

namespace GeoServer.Controllers
{
    public class CATOesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: CATOes
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string AB, string CD, string EF, string HIJ, string Name, int? Page)
        {
            var catoes = db.CATOes
                .Where(c => true);

            ViewBag.ABFilter = AB;
            ViewBag.CDFilter = CD;
            ViewBag.EFFilter = EF;
            ViewBag.HIJFilter = HIJ;
            ViewBag.NameFilter = Name;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";

            if (!string.IsNullOrEmpty(AB))
            {
                catoes = catoes.Where(c => c.AB.Contains(AB));
            }
            if (!string.IsNullOrEmpty(CD))
            {
                catoes = catoes.Where(c => c.CD.Contains(CD));
            }
            if (!string.IsNullOrEmpty(EF))
            {
                catoes = catoes.Where(c => c.EF.Contains(EF));
            }
            if (!string.IsNullOrEmpty(HIJ))
            {
                catoes = catoes.Where(c => c.HIJ.Contains(HIJ));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                catoes = catoes.Where(c => c.Name.Contains(Name));
            }

            switch (SortOrder)
            {
                case "Name":
                    catoes = catoes.OrderBy(c => c.Name);
                    break;
                case "NameDesc":
                    catoes = catoes.OrderByDescending(c => c.Name);
                    break;
                default:
                    catoes = catoes.OrderBy(c => c.Id);
                    break;
            }

            int PageSize = 100;
            int PageNumber = (Page ?? 1);

            return View(catoes.ToPagedList(PageNumber, PageSize));
        }

        // GET: CATOes/Details/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATO cATO = await db.CATOes.FindAsync(id);
            if (cATO == null)
            {
                return HttpNotFound();
            }
            return View(cATO);
        }

        // GET: CATOes/Create
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CATOes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,AB,CD,EF,HIJ,Name")] CATO cATO)
        {
            if (ModelState.IsValid)
            {
                db.CATOes.Add(cATO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cATO);
        }

        // GET: CATOes/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATO cATO = await db.CATOes.FindAsync(id);
            if (cATO == null)
            {
                return HttpNotFound();
            }
            return View(cATO);
        }

        // POST: CATOes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AB,CD,EF,HIJ,Name")] CATO cATO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cATO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cATO);
        }

        // GET: CATOes/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATO cATO = await db.CATOes.FindAsync(id);
            if (cATO == null)
            {
                return HttpNotFound();
            }
            return View(cATO);
        }

        // POST: CATOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CATO cATO = await db.CATOes.FindAsync(id);
            db.CATOes.Remove(cATO);
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

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> Files)
        {
            string report = "";
            foreach (var file in Files)
            {
                report += "Файл\t" + file.FileName + ":<br/>";
                int count = 0;
                if (file != null && file.ContentLength > 0)
                {
                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes((int)file.InputStream.Length);
                    string result = Encoding.Default.GetString(binData);
                    var strings = result.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in strings)
                    {
                        try
                        {
                            var c = s.Split("\t".ToCharArray());
                            CATO cato = new CATO()
                            {
                                AB = c[0],
                                CD = c[1],
                                EF = c[2],
                                HIJ = c[3],
                                Name = c[4]
                            };
                            db.CATOes.Add(cato);
                            count++;
                        }
                        catch
                        {
                            report += "строка\t\"" + s + "\" не распознана,<br/>";
                        }
                    }
                }
                db.SaveChanges();
                report += "загружено строк\t" + count.ToString() + ".<br/><br/>";
            }
            ViewBag.Report = report;
            return View();
            //string report = "";
            //foreach (var file in Files)
            //{
            //    report += "Файл\t" + file.FileName + ":<br/>";
            //    int count = 0;
            //    if (file != null && file.ContentLength > 0)
            //    {
            //        BinaryReader b = new BinaryReader(file.InputStream);
            //        byte[] binData = b.ReadBytes((int)file.InputStream.Length);
            //        string result = Encoding.Default.GetString(binData);
            //        var strings = result.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //        foreach (string s in strings)
            //        {
            //            try
            //            {
            //                var c = s.Split("\t".ToCharArray());
            //                int id = Convert.ToInt32(c[0]);
            //                int cr = Convert.ToInt32(c[1]);
            //                field field = db.fields.Where(f => f.gid == id).FirstOrDefault();
            //                field.croprotation = cr;
            //                db.Entry(field).State = EntityState.Modified;
            //                count++;
            //            }
            //            catch
            //            {
            //                report += "строка\t\"" + s + "\" не распознана,<br/>";
            //            }
            //        }
            //    }
            //    db.SaveChanges();
            //    report += "загружено строк\t" + count.ToString() + ".<br/><br/>";
            //}
            //ViewBag.Report = report;
            //return View();
        }
    }
}
