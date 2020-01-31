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
    public class CropRotationsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: CropRotations
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string CropRotationName, int? fieldgid, int? Year, int? Page)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            var cropRotations = db.CropRotations
                .Include(c => c.CropRotationType)
                .Include(c => c.field)
                .Where(c => true);

            ViewBag.CropRotationNameFilter = CropRotationName;
            ViewBag.fieldgidFilter = fieldgid;
            ViewBag.YearFilter = Year;

            ViewBag.CropRotationNameSort = SortOrder == "CropRotationName" ? "CropRotationNameDesc" : "CropRotationName";
            ViewBag.fieldgidSort = SortOrder == "fieldgid" ? "fieldgidDesc" : "fieldgid";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";

            if (!string.IsNullOrEmpty(CropRotationName))
            {
                cropRotations = cropRotations.Where(c => c.CropRotationType.NameRU == CropRotationName || c.CropRotationType.NameKK == CropRotationName || c.CropRotationType.NameEN == CropRotationName);
            }
            if (fieldgid != null)
            {
                cropRotations = cropRotations.Where(c => c.field.gid == fieldgid);
            }
            if (Year != null)
            {
                cropRotations = cropRotations.Where(c => c.Year == Year);
            }
            
            switch(SortOrder)
            {
                case "CropRotationName":
                    if(ViewBag.Language == "ru")
                    {
                        cropRotations = cropRotations.OrderBy(c => c.CropRotationType.NameRU);
                    }
                    if (ViewBag.Language == "en")
                    {
                        cropRotations = cropRotations.OrderBy(c => c.CropRotationType.NameEN);
                    }
                    if (ViewBag.Language == "kk")
                    {
                        cropRotations = cropRotations.OrderBy(c => c.CropRotationType.NameKK);
                    }
                    break;
                case "CropRotationNameDesc":
                    if (ViewBag.Language == "ru")
                    {
                        cropRotations = cropRotations.OrderByDescending(c => c.CropRotationType.NameRU);
                    }
                    if (ViewBag.Language == "en")
                    {
                        cropRotations = cropRotations.OrderByDescending(c => c.CropRotationType.NameEN);
                    }
                    if (ViewBag.Language == "kk")
                    {
                        cropRotations = cropRotations.OrderByDescending(c => c.CropRotationType.NameKK);
                    }
                    break;
                case "fieldgid":
                    cropRotations = cropRotations.OrderBy(c => c.field.gid);
                    break;
                case "fieldgidDesc":
                    cropRotations = cropRotations.OrderByDescending(c => c.field.gid);
                    break;
                case "Year":
                    cropRotations = cropRotations.OrderBy(c => c.Year);
                    break;
                case "YearDesc":
                    cropRotations = cropRotations.OrderByDescending(c => c.Year);
                    break;
                default:
                    cropRotations = cropRotations.OrderBy(c => c.Id);
                    break;
            }

            int PageSize = 100;
            int PageNumber = (Page ?? 1);

            if (ViewBag.Language == "ru")
            {
                ViewBag.CropRotationNames = new SelectList(db.CropRotationTypes.ToList().OrderBy(c => c.NameRU), "NameRU", "NameRU");
            }
            if (ViewBag.Language == "en")
            {
                ViewBag.CropRotationNames = new SelectList(db.CropRotationTypes.ToList().OrderBy(c => c.NameEN), "NameEN", "NameEN");
            }
            if (ViewBag.Language == "kk")
            {
                ViewBag.CropRotationNames = new SelectList(db.CropRotationTypes.ToList().OrderBy(c => c.NameKK), "NameKK", "NameKK");
            }

            return View(cropRotations.ToPagedList(PageNumber, PageSize));
        }

        // GET: CropRotations/Details/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropRotation cropRotation = db.CropRotations
                .Where(c => c.Id == id)
                .Include(c => c.CropRotationType)
                .FirstOrDefault();
            if (cropRotation == null)
            {
                return HttpNotFound();
            }
            return View(cropRotation);
        }

        // GET: CropRotations/Create
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Create()
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            if (ViewBag.Language == "ru")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameRU");
            }
            if (ViewBag.Language == "en")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameEN");
            }
            if (ViewBag.Language == "kk")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameKK");
            }
            CropRotation cropRotation = new CropRotation() {
                Year = DateTime.Today.Year
            };
            return View(cropRotation);
        }

        // POST: CropRotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,CropRotationTypeId,fieldgid,Year")] CropRotation cropRotation)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            int field_count = db.fields.Where(f => f.gid == cropRotation.fieldgid).Count();
            if ((ModelState.IsValid) && (field_count > 0))
            {
                db.CropRotations.Add(cropRotation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            if (ViewBag.Language == "ru")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameRU", cropRotation.CropRotationTypeId);
            }
            if (ViewBag.Language == "en")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameEN", cropRotation.CropRotationTypeId);
            }
            if (ViewBag.Language == "kk")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameKK", cropRotation.CropRotationTypeId);
            }
            return View(cropRotation);
        }

        // GET: CropRotations/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropRotation cropRotation = await db.CropRotations.FindAsync(id);
            if (cropRotation == null)
            {
                return HttpNotFound();
            }
            if (ViewBag.Language == "ru")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameRU", cropRotation.CropRotationTypeId);
            }
            if (ViewBag.Language == "en")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameEN", cropRotation.CropRotationTypeId);
            }
            if (ViewBag.Language == "kk")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameKK", cropRotation.CropRotationTypeId);
            }
            return View(cropRotation);
        }

        // POST: CropRotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CropRotationTypeId,fieldgid,Year")] CropRotation cropRotation)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            int field_count = db.fields.Where(f => f.gid == cropRotation.fieldgid).Count();
            if ((ModelState.IsValid) && (field_count > 0))
            {
                db.Entry(cropRotation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (ViewBag.Language == "ru")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameRU", cropRotation.CropRotationTypeId);
            }
            if (ViewBag.Language == "en")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameEN", cropRotation.CropRotationTypeId);
            }
            if (ViewBag.Language == "kk")
            {
                ViewBag.CropRotationTypeId = new SelectList(db.CropRotationTypes, "Id", "NameKK", cropRotation.CropRotationTypeId);
            }
            return View(cropRotation);
        }

        // GET: CropRotations/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CropRotation cropRotation = db.CropRotations
                .Where(c => c.Id == id)
                .Include(c => c.CropRotationType)
                .FirstOrDefault();
            if (cropRotation == null)
            {
                return HttpNotFound();
            }
            return View(cropRotation);
        }

        // POST: CropRotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CropRotation cropRotation = await db.CropRotations.FindAsync(id);
            db.CropRotations.Remove(cropRotation);
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
