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
    public class NDVIsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: NDVIs
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Index(string SortOrder, int? fieldgid, int? Year, int? Month, int? Day, int? Page)
        {
            //var nDVIs = db.NDVIs.Include(n => n.field);
            //return View(await nDVIs.ToListAsync());
            var nDVIs = db.NDVIs
                .Where(n => true)
                .Include(n => n.field);

            ViewBag.fieldgidFilter = fieldgid;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;
            ViewBag.DayFilter = Day;

            ViewBag.fieldgidSort = SortOrder == "fieldgid" ? "fieldgidDesc" : "fieldgid";
            ViewBag.DateSort = SortOrder == "Date" ? "DateDesc" : "Date";

            if (fieldgid != null)
            {
                nDVIs = nDVIs.Where(n => n.field.gid == fieldgid);
            }
            if (Year != null)
            {
                nDVIs = nDVIs.Where(n => n.Date.Year == Year);
            }
            if (Month != null)
            {
                nDVIs = nDVIs.Where(n => n.Date.Month == Month);
            }
            if (Day != null)
            {
                nDVIs = nDVIs.Where(n => n.Date.Day == Day);
            }

            switch (SortOrder)
            {
                case "fieldgid":
                    nDVIs = nDVIs.OrderBy(n => n.field.gid);
                    break;
                case "fieldgidDesc":
                    nDVIs = nDVIs.OrderByDescending(n => n.field.gid);
                    break;
                case "Date":
                    nDVIs = nDVIs.OrderBy(n => n.Date);
                    break;
                case "DateDesc":
                    nDVIs = nDVIs.OrderByDescending(n => n.Date);
                    break;
                default:
                    nDVIs = nDVIs.OrderBy(n => n.Id);
                    break;
            }

            int PageSize = 100;
            int PageNumber = (Page ?? 1);

            return View(nDVIs.ToPagedList(PageNumber, PageSize));
        }

        // GET: NDVIs/Details/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NDVI nDVI = await db.NDVIs.FindAsync(id);
            if (nDVI == null)
            {
                return HttpNotFound();
            }
            return View(nDVI);
        }

        // GET: NDVIs/Create
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Create()
        {
            NDVI nDVI = new NDVI()
            {
                Date = DateTime.Today
            };
            //ViewBag.fieldgid = new SelectList(db.fields, "gid", "gid");
            return View(nDVI);
        }

        // POST: NDVIs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,fieldgid,Count,Min,Max,Range,Mean,STD,Sum")] NDVI nDVI)
        {
            int Day = Convert.ToInt32(Request.Form["Day"]);
            int Month = Convert.ToInt32(Request.Form["Month"]);
            int Year = Convert.ToInt32(Request.Form["Year"]);
            try
            {
                nDVI.Date = new DateTime(Year, Month, Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        nDVI.Date = new DateTime(Year, Month, Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            int field_count = db.fields.Where(f => f.gid == nDVI.fieldgid).Count();
            if ((ModelState.IsValid) && (field_count > 0))
            {
                db.NDVIs.Add(nDVI);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //ViewBag.fieldgid = new SelectList(db.fields, "gid", "gid", nDVI.fieldgid);
            return View(nDVI);
        }

        // GET: NDVIs/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NDVI nDVI = await db.NDVIs.FindAsync(id);
            if (nDVI == null)
            {
                return HttpNotFound();
            }
            //ViewBag.fieldgid = new SelectList(db.fields, "gid", "gid", nDVI.fieldgid);
            return View(nDVI);
        }

        // POST: NDVIs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,fieldgid,Count,Min,Max,Range,Mean,STD,Sum")] NDVI nDVI)
        {
            int Day = Convert.ToInt32(Request.Form["Day"]);
            int Month = Convert.ToInt32(Request.Form["Month"]);
            int Year = Convert.ToInt32(Request.Form["Year"]);
            try
            {
                nDVI.Date = new DateTime(Year, Month, Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        nDVI.Date = new DateTime(Year, Month, Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            int field_count = db.fields.Where(f => f.gid == nDVI.fieldgid).Count();
            if ((ModelState.IsValid) && (field_count > 0))
            {
                db.Entry(nDVI).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.fieldgid = new SelectList(db.fields, "gid", "gid", nDVI.fieldgid);
            return View(nDVI);
        }

        // GET: NDVIs/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NDVI nDVI = await db.NDVIs.FindAsync(id);
            if (nDVI == null)
            {
                return HttpNotFound();
            }
            return View(nDVI);
        }

        // POST: NDVIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NDVI nDVI = await db.NDVIs.FindAsync(id);
            db.NDVIs.Remove(nDVI);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Calculate()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public ActionResult Calculate(int? no)
        {
            string report = DateTime.Now.ToString() + "<br/>";

            //db.DoneNDVIs.RemoveRange(db.DoneNDVIs);
            //// районы
            //foreach (admpol2 admpol2 in db.Admpol2.ToList())
            //{
            //    var fields = db.fields
            //        .Where(f => f.catoid == admpol2.catoid);
            //    List<int> fieldgids = fields
            //        .Select(f => f.gid)
            //        .ToList();
            //    var NDVIs = db.NDVIs
            //        .Where(n => fieldgids.Contains(n.fieldgid))
            //        .OrderBy(n => n.Date);

            //    List<decimal> values = new List<decimal>();
            //    List<int> counts = new List<int>();

            //    List<DateTime> dates = NDVIs
            //        .Select(n => n.Date)
            //        .Distinct()
            //        .ToList();
            //    foreach (DateTime date in dates)
            //    {
            //        List<decimal> ndvis = NDVIs
            //            .Where(n => n.Date == date)
            //            .Select(n => n.Mean)
            //            .ToList();
            //        values.Add(ndvis.Sum());
            //        counts.Add(ndvis.Count());
            //    }
            //    for (int i = 0; i < values.Count; i++)
            //    {
            //        values[i] = values[i] / counts[i];
            //        DoneNDVI doneNDVI = new DoneNDVI()
            //        {
            //            catoid = admpol2.catoid,
            //            Date = dates[i],
            //            MeanAverage = values[i]
            //        };
            //        db.DoneNDVIs.Add(doneNDVI);
            //    }
            //}

            //// области
            //foreach (admpol1 admpol1 in db.Admpol1.ToList())
            //{
            //    // КАТО области
            //    CATO admpol1_CATO = db.CATOes
            //           .Where(c => c.Id == admpol1.catoid)
            //           .FirstOrDefault();
            //    // КАТО районов области
            //    List<int> catoids = db.CATOes
            //        .Where(c => c.AB == admpol1_CATO.AB && c.CD != "00" && c.EF == "00")
            //        .Select(c => c.Id)
            //        .ToList();
            //    // поля области
            //    List<int> fieldgids = db.fields
            //        .Where(f => catoids.Contains((int)f.catoid))
            //        .Select(f => f.gid)
            //        .ToList();

            //    List<decimal> values = new List<decimal>();
            //    List<int> counts = new List<int>();

            //    List<DateTime> dates = db.DoneNDVIs
            //        .Select(d => d.Date)
            //        .Distinct()
            //        .ToList();

            //    foreach (DateTime date in dates)
            //    {
            //        List<decimal> ndvi_value = db.NDVIs
            //            .Where(n => n.Date == date && fieldgids.Contains(n.fieldgid))
            //            .Select(n => n.Mean)
            //            .ToList();
            //        if (ndvi_value.Count != 0)
            //        {
            //            values.Add(ndvi_value.Sum());
            //            counts.Add(ndvi_value.Count());
            //        }
            //    }
            //    for (int i = 0; i < values.Count; i++)
            //    {
            //        values[i] = values[i] / counts[i];
            //        DoneNDVI doneNDVI = new DoneNDVI()
            //        {
            //            catoid = admpol1.catoid,
            //            Date = dates[i],
            //            MeanAverage = values[i]
            //        };
            //        db.DoneNDVIs.Add(doneNDVI);
            //    }
            //}







            //db.CropConditionNDVIs
            //    .RemoveRange(db.CropConditionNDVIs);

            //// районы

            //// все даты
            ////List<DateTime> dates = NDVIs
            //List<DateTime> dates = db.NDVIs
            //    .Select(n => n.Date)
            //    //.OrderBy(n => n.Date)
            //    .Distinct()
            //    .ToList();
            //dates = dates
            //    .OrderBy(d => d.Date)
            //    .ToList();
            //foreach (admpol2 admpol2 in db.Admpol2.ToList())
            //{
            //    //string AB = db.CATOes
            //    //    .Where(c => c.Id == admpol2.catoid)
            //    //    .Select(c => c.AB)
            //    //    .FirstOrDefault();
            //    //if(AB == "11")
            //    //{

            //    //}

            //    // поля района
            //    var fields = db.fields
            //        .Where(f => f.catoid == admpol2.catoid)
            //        .ToList();
            //    // id полей района
            //    List<int> fieldgids = fields
            //        .Select(f => f.gid)
            //        .ToList();
            //    //// NDVIы района
            //    //var NDVIs = db.NDVIs
            //    //    .Where(n => fieldgids.Contains(n.fieldgid))
            //    //    .OrderBy(n => n.Date)
            //    //    .ToList();

            //    // типы состояния посева
            //    var CropConditionTypes = db.CropConditionTypes
            //        .OrderBy(c => c.Max)
            //        .ToList();
            //    if (fields.Count > 0)
            //    {
            //        foreach (DateTime date in dates)
            //        {
            //            foreach (CropConditionType cctype in CropConditionTypes.ToList())
            //            {
            //                decimal max = 1, min = 0;
            //                max = cctype.Max;
            //                if (cctype.Max == CropConditionTypes[0].Max)
            //                {
            //                    min = -1;
            //                }
            //                else
            //                {
            //                    min = CropConditionTypes
            //                        .Where(c => c.Max < max)
            //                        .LastOrDefault()
            //                        .Max;
            //                }
            //                CropConditionNDVI newccNDVI = new CropConditionNDVI()
            //                {
            //                    catoid = admpol2.catoid,
            //                    CropConditionTypeId = cctype.Id,
            //                    Date = date
            //                };
            //                // поля района с подходящими по типу состояния посева NDVI района на дату
            //                //List<int> needndvis = NDVIs
            //                List<int> needndvis = db.NDVIs
            //                    .Where(n => n.Date == date && n.Mean > min && n.Mean <= max && fieldgids.Contains(n.fieldgid))
            //                    .Select(n => n.fieldgid)
            //                    .ToList();
            //                newccNDVI.Value = fields
            //                    .Where(f => needndvis.Contains(f.gid))
            //                    .Select(f => f.area)
            //                    .Sum();
            //                if (needndvis.Count > 0)
            //                {
            //                    db.CropConditionNDVIs.Add(newccNDVI);
            //                }
            //            }
            //        }
            //    }
            //}

            //// области
            //// ...




            ////db.FieldNDVIJsons.RemoveRange(db.FieldNDVIJsons);

            //db.Configuration.AutoDetectChangesEnabled = false;

            //// все даты
            //List<DateTime> dates = db.NDVIs
            //    .Select(n => n.Date)
            //    .Distinct()
            //    .ToList();
            //dates = dates
            //    .OrderBy(d => d.Date)
            //    .ToList();
            //// все районы (КАТО)
            //List<int> catoes = db.Admpol2
            //    .Select(a => (int)a.catoid)
            //    .ToList();
            //// все поля (json)
            ////var jsonstr = new WebClient().DownloadString("http://geoserver.info.tm:8080/geoserver/localhost/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=localhost:fields&outputFormat=text/javascript&format_options=callback:getJson");
            //var jsonstr = new WebClient().DownloadString("http://localhost:8080/geoserver/localhost/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=localhost:fields&outputFormat=text/javascript&format_options=callback:getJson");
            //jsonstr = jsonstr.Remove(jsonstr.Length - 1);
            //jsonstr = jsonstr.Remove(0, 8);
            ////dynamic jsonObjall = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstr);
            ////for (int i = jsonObjall["features"].Count - 1; i >= 0; i--)
            ////{
            ////    jsonObjall["features"][i]["properties"].Add("NDVI", 0);
            ////}
            ////var ndvis = db.NDVIs.ToList();
            //foreach (int catoid in catoes)
            //{
            //    int fcount = db.fields
            //        .Where(f => f.catoid == catoid)
            //        .Count();
            //    if(fcount==0)
            //    {
            //        continue;
            //    }
            //    int count = db.FieldNDVIJsons
            //        .Where(f => f.catoid == catoid)
            //        .Count();
            //    if (count != 0)
            //    {
            //        continue;
            //    }
            //    // поля КАТО
            //    dynamic jsonObjCATO = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstr);
                
            //    for (int i = jsonObjCATO["features"].Count - 1; i >= 0; i--)
            //    {
            //        if(jsonObjCATO["features"][i]["properties"]["catoid"].Value != catoid)
            //        {
            //            jsonObjCATO["features"][i].Remove();
            //        }
            //        else
            //        {
            //            jsonObjCATO["features"][i]["properties"].Add("NDVI", 0);
            //            //fields.Add(jsonObjCATO["features"][i]["properties"]["idfrommap"]);
            //        }
            //    }
            //    List<int> fields = db.fields
            //        .Where(f => f.catoid == catoid)
            //        .Select(f => f.gid)
            //        .ToList();
            //    //if (jsonObjCATO["features"].Count > 0)
            //    //{
            //    var ndviscato = db.NDVIs
            //        .Where(n => fields.Contains(n.fieldgid))
            //        .ToList();
                
            //    foreach (DateTime date in dates)
            //    {
            //        //int count = db.FieldNDVIJsons
            //        //    .Where(f => f.catoid == catoid && f.Date == date)
            //        //    .Count();
            //        //if(count == 0)
            //        //{
            //        var ndvis = ndviscato
            //            .Where(n => n.Date == date)
            //            .ToList();
            //        for (int i = jsonObjCATO["features"].Count - 1; i >= 0; i--)
            //        {
            //            int fieldgid = Convert.ToInt32(jsonObjCATO["features"][i]["properties"]["idfrommap"].Value);
            //            NDVI ndvi = ndvis
            //                //.Where(n => n.Date == date && n.fieldgid == fieldgid)
            //                .Where(n => n.fieldgid == fieldgid)
            //                .FirstOrDefault();
            //            if(ndvi != null)
            //            {
            //                jsonObjCATO["features"][i]["properties"]["NDVI"].Value = ndvi.Mean;
            //            }
            //        }
            //        //string jsonstrCATO = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObjCATO);
            //        FieldNDVIJson fnj = new FieldNDVIJson()
            //        {
            //            catoid = catoid,
            //            Date = date,
            //            Json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObjCATO)
            //        };
            //        db.FieldNDVIJsons.Add(fnj);
            //        ndvis = null;
            //        GC.Collect();
            //        //}
                        
            //    }
            //    //}
            //    ndviscato = null;
            //    jsonObjCATO = null;
            //    fields = null;
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    db.SaveChangesAsync();
            //}


            db.SaveChangesAsync();
            report += DateTime.Now.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
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
