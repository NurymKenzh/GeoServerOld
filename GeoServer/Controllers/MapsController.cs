using GeoServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GeoServer.Controllers
{
    public class MapsController : Controller
    {
        const string geoserverURL = "http://92.46.36.100:8080/geoserver/",
            geoserverWorkspace = "GeoServerOld";

        private NpgsqlContext db = new NpgsqlContext();

        // GET: Maps
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GrainProduction()
        {
            var mapcatoes = db.Admpol1
                .Select(a => a.catoid)
                .ToList();
            ViewBag.CATOobl = new SelectList(db.CATOes
                .Where(c => (c.CD == "00") && mapcatoes.Contains(c.Id))
                .OrderBy(c => c.Name)
                ,
                    "Id",
                    "Name");
            ViewBag.NDVIcatoobl = ViewBag.CATOobl;
            ViewBag.CropsConditioncatoobl = ViewBag.CATOobl;
            ViewBag.SownAreacatoobl = ViewBag.CATOobl;
            List<SelectListItem> MapSources = new List<SelectListItem>();
            var MapSourcesData = new[]{
                    new SelectListItem{ Value="OpenStreetMap",Text="Open Street Map", Selected = true},
                    new SelectListItem{ Value="ArcGIS",Text="ArcGIS"},
                    new SelectListItem{ Value="Bing",Text="Bing"},
                    };
            MapSources = MapSourcesData.OrderBy(s => s.Text).ToList();
            ViewBag.MapSources = MapSources;
            ViewBag.geoserverURL = geoserverURL;
            ViewBag.geoserverWorkspace = geoserverWorkspace;
            return View();
        }

        public ActionResult Irrigation()
        {
            List<SelectListItem> MapSources = new List<SelectListItem>();
            var MapSourcesData = new[]{
                    new SelectListItem{ Value="OpenStreetMap",Text="Open Street Map"},
                    new SelectListItem{ Value="OpenCycleMap",Text="Open Cycle Map", Selected = true},
                    new SelectListItem{ Value="ArcGIS",Text="ArcGIS"},
                    new SelectListItem{ Value="Bing",Text="Bing"},
                    };
            MapSources = MapSourcesData.OrderBy(s => s.Text).ToList();
            ViewBag.MapSources = MapSources;
            return View();
        }

        public ActionResult MSW()
        {
            List<SelectListItem> MapSources = new List<SelectListItem>();
            var MapSourcesData = new[]{
                    new SelectListItem{ Value="OpenStreetMap",Text="Open Street Map"},
                    new SelectListItem{ Value="OpenCycleMap",Text="Open Cycle Map", Selected = true},
                    new SelectListItem{ Value="ArcGIS",Text="ArcGIS"},
                    new SelectListItem{ Value="Bing",Text="Bing"},
                    };
            MapSources = MapSourcesData.OrderBy(s => s.Text).ToList();
            ViewBag.MapSources = MapSources;
            return View();
        }

        [HttpPost]
        public ActionResult GetCATOName(int catoid)
        {
            string CATOName = db.CATOes
                .Where(c => c.Id == catoid)
                .Select(c => c.Name)
                .FirstOrDefault();
            return Json(new { CATOName = CATOName });
        }

        [HttpPost]
        public ActionResult GetCropRotation(int fieldidfrommap, int year)
        {
            ViewBag.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            CropRotation CropRotation = db.CropRotations
                .Where(c => c.fieldgid == fieldidfrommap && c.Year == year)
                .FirstOrDefault();
            string CropRotationTypeName = "";
            CropRotationType CropRotationType = null;
            if(CropRotation != null)
            {
                CropRotationType = db.CropRotationTypes
                    .Where(c => c.Id == CropRotation.CropRotationTypeId)
                    .FirstOrDefault();
            }
            if(CropRotationType != null)
            {
                if (ViewBag.Language == "ru")
                {
                    CropRotationTypeName = CropRotationType.NameRU;
                }
                if (ViewBag.Language == "en")
                {
                    CropRotationTypeName = CropRotationType.NameEN;
                }
                if (ViewBag.Language == "kk")
                {
                    CropRotationTypeName = CropRotationType.NameKK;
                }
            }
            return Json(new { CropRotationTypeName = CropRotationTypeName });
        }

        [HttpPost]
        public ActionResult GetFieldArea(int fieldidfrommap)
        {
            string FieldArea = "";
            decimal area = db.fields
                .Where(f => f.idfrommap == fieldidfrommap)
                .FirstOrDefault()
                .area;
            FieldArea = Math.Round(area/10000).ToString();
            return Json(new { FieldArea = FieldArea });
        }

        [HttpPost]
        public ActionResult GetCATORayons(int oblcatoid)
        {
            var mapcatoes = db.Admpol2
                .Select(a => a.catoid)
                .ToList();
            string AB = db.CATOes
                .Where(c => c.Id == oblcatoid)
                .Select(c => c.AB)
                .FirstOrDefault();
            var catoes = db.CATOes
                .Where(c => c.AB == AB && c.CD != "00" && c.EF == "00" && mapcatoes.Contains(c.Id))
                .ToList();
            catoes.Add(new CATO { Id = -1, Name = "" });
            var catoesn = catoes.OrderBy(c => c.Name).ToList();
            JsonResult rayons = new JsonResult();
            rayons.Data = catoesn;
            return rayons;
        }

        [HttpPost]
        public ActionResult GetCATORayonsNotEmpty(int oblcatoid)
        {
            var mapcatoes = db.Admpol2
                .Select(a => a.catoid)
                .ToList();
            string AB = db.CATOes
                .Where(c => c.Id == oblcatoid)
                .Select(c => c.AB)
                .FirstOrDefault();
            var catoes = db.CATOes
                .Where(c => c.AB == AB && c.CD != "00" && c.EF == "00" && mapcatoes.Contains(c.Id))
                .ToList();
            var catoesn = catoes.OrderBy(c => c.Name).ToList();
            JsonResult rayons = new JsonResult();
            rayons.Data = catoesn;
            return rayons;
        }

        public ActionResult GetNDVIChartImage(int? Year, int? MonthStart, int? DayStart, int? MonthFinish, int? DayFinish, int? CATOId)
        {
            if(CATOId == null)
            {
                return null;
            }

            DateTime Start = DateTime.Today;
            int? Month = MonthStart,
                Day = DayStart;
            try
            {
                Start = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Start = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }
            DateTime Finish = DateTime.Today;
            Month = MonthFinish;
            Day = DayFinish;
            try
            {
                Finish = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Finish = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            List<decimal> values = db.DoneNDVIs
                .Where(d => d.Date >= Start && d.Date <= Finish && d.catoid == CATOId)
                .OrderBy(d => d.Date)
                .Select(d => d.MeanAverage)
                .ToList();
            List<DateTime> dates = db.DoneNDVIs
                .Where(d => d.Date >= Start && d.Date <= Finish && d.catoid == CATOId)
                .OrderBy(d => d.Date)
                .Select(d => d.Date)
                .ToList();
            string title = db.CATOes
                .Where(c => c.Id == CATOId)
                .Select(c => c.Name)
                .ToList()
                .FirstOrDefault();

            List<decimal> values_avg = new List<decimal>();
            List<DateTime> dates_avg = new List<DateTime>();
            foreach(DateTime date in dates)
            {
                List<DoneNDVI> dndvi = db.DoneNDVIs
                    .Where(d => d.catoid == CATOId)
                    .ToList();
                decimal value_a = dndvi
                    .Where(d => d.Date.Month == date.Month && d.Date.Day == date.Day)
                    .Select(d => d.MeanAverage)
                    .Average();
                values_avg.Add(value_a);
                dates_avg.Add(date);
            }

            int min_year = db.DoneNDVIs.Min(d => d.Date).Year;
            int max_year = db.DoneNDVIs.Max(d => d.Date).Year;

            string theme = "<Chart BackColor=\"#C9DC87\" BackGradientStyle=\"TopBottom\" BorderColor=\"181, 64, 1\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"BrightPastel\">\r\n  <ChartAreas>\r\n    <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n      <AxisY LineColor=\"64, 64, 64, 64\">\r\n        <MajorGrid Interval=\"Auto\" LineColor=\"64, 64, 64, 64\" />\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisY>\r\n      <AxisX LineColor=\"64, 64, 64, 64\">\r\n        <MajorGrid LineColor=\"64, 64, 64, 64\" />\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisX>\r\n      <Area3DStyle Inclination=\"15\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"10\" WallWidth=\"0\" />\r\n    </ChartArea>\r\n  </ChartAreas>\r\n  <Legends>\r\n    <Legend _Template_=\"All\" Alignment=\"Center\" BackColor=\"Transparent\" Docking=\"Bottom\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit =\"True\" LegendStyle=\"Row\">\r\n    </Legend>\r\n  </Legends>\r\n  <BorderSkin SkinStyle=\"Emboss\" />\r\n</Chart>";
            var chart_NDVI = new Chart(width: 800, height: 500, theme: theme)
            .AddTitle(title)
            .AddSeries(
                name: Resources.NDVITexts.AverageFor + Year.ToString(),
                chartType: "Line",
                xValue: dates,
                yValues: values,
                legend: "legend")
            .AddSeries(
                name: Resources.NDVITexts.AverageLongTerm + "(" + min_year.ToString() + " - " + max_year.ToString() + ")",
                chartType: "Line",
                xValue: dates_avg,
                yValues: values_avg,
                legend: "legend"
                )
            .AddLegend(name: "legend");

            return File(chart_NDVI.ToWebImage().GetBytes(), "image/jpeg");
        }

        [HttpPost]
        public ActionResult GetCropConditionDate(int? Year, int? MonthStart, int? DayStart, int? DatePosition)
        {
            DateTime Start = DateTime.Today;
            int? Month = MonthStart,
                Day = DayStart;
            try
            {
                Start = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Start = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            IList<DateTime> Dates = db.CropConditionNDVIs
                .Where(c => c.Date >= Start && c.Date.Year == Year)
                .Select(c => c.Date)
                .Distinct()
                .ToList();
            Dates = Dates.OrderBy(d => d.Date).ToList();
            DateTime Date = Dates[DatePosition == null ? 0 : (int)DatePosition];

            return Json(new { Date = Date.ToShortDateString() });
        }

        public ActionResult GetCropsConditionChartImage(int? Year, int? MonthStart, int? DayStart, /*int? MonthFinish, int? DayFinish, */int? CATOId, int? DatePosition)
        {
            if (CATOId == null)
            {
                return null;
            }

            DateTime Start = DateTime.Today;
            int? Month = MonthStart,
                Day = DayStart;
            try
            {
                Start = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Start = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            List<decimal> values= new List<decimal>();
            List<string> names = new List<string>();
            
            IList<DateTime> Dates = db.CropConditionNDVIs
                .Where(c => c.Date >= Start && c.Date.Year == Year)
                .Select(c => c.Date)
                .Distinct()
                .ToList();
            Dates = Dates.OrderBy(d => d.Date).ToList();
            DateTime Date = Dates[DatePosition == null ? 0 : (int) DatePosition];

            string title = db.CATOes
                .Where(c => c.Id == CATOId)
                .Select(c => c.Name)
                .ToList()
                .FirstOrDefault();

            foreach (CropConditionType cctype in db.CropConditionTypes.OrderBy(c => c.Max).ToList())
            {
                CropConditionNDVI ccNDVI = db.CropConditionNDVIs
                    .Where(c => c.catoid == CATOId && c.CropConditionTypeId == cctype.Id && c.Date == Date)
                    .Include(c => c.CropConditionType)
                    .FirstOrDefault();
                if(ccNDVI!=null)
                {
                    if(ccNDVI.Value > 0)
                    {
                        values.Add(ccNDVI.Value);
                        names.Add(ccNDVI.CropConditionType.Name);
                    }
                }
                
            }

            title += " (" + Date.ToShortDateString() + ")";

            string theme = "<Chart PaletteCustomColors=\"255,0,0; 255,127,39; 255,200,14; 181,230,29; 0,255,0; 34,177,76; 63,72,204\" BackColor=\"#C9DC87\" BackGradientStyle=\"TopBottom\" BorderColor=\"181, 64, 1\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"None\">\r\n  <ChartAreas>\r\n    <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n      <AxisY LineColor=\"64, 64, 64, 64\">\r\n        <MajorGrid Interval=\"Auto\" LineColor=\"64, 64, 64, 64\" />\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisY>\r\n      <AxisX LineColor=\"64, 64, 64, 64\">\r\n        <MajorGrid LineColor=\"64, 64, 64, 64\" />\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisX>\r\n      <Area3DStyle Inclination=\"15\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"10\" WallWidth=\"0\" />\r\n    </ChartArea>\r\n  </ChartAreas>\r\n  <Legends>\r\n    <Legend _Template_=\"All\" Alignment=\"Center\" BackColor=\"Transparent\" Docking=\"Right\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit =\"True\" LegendStyle=\"Column\">\r\n    </Legend>\r\n  </Legends>\r\n  <BorderSkin SkinStyle=\"Emboss\" />\r\n</Chart>";
            var chart_CropsCondition = new Chart(width: 450, height: 350, theme: theme)
            .AddTitle(title)
            .AddSeries(
                chartType: "Pie",
                xValue: names,
                yValues: values)
            .AddLegend();

            return File(chart_CropsCondition.ToWebImage().GetBytes(), "image/jpeg");
        }

        [HttpPost]
        public ActionResult GetCropConditionDatesCount(int? Year, int? MonthStart, int? DayStart, int? MonthFinish, int? DayFinish, int? CATOId)
        {
            DateTime Start = DateTime.Today;
            int? Month = MonthStart,
                Day = DayStart;
            try
            {
                Start = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Start = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }
            DateTime Finish = DateTime.Today;
            Month = MonthFinish;
            Day = DayFinish;
            try
            {
                Finish = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Finish = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            int count = 0;

            count = db.CropConditionNDVIs
                .Where(c => c.Date >= Start && c.Date <= Finish && c.catoid == CATOId)
                .Select(c => c.Date)
                .Distinct()
                .Count();

            return Json(new { Count = count });
        }

        [HttpPost]
        public ActionResult GetCATOFields(int? Year, int? MonthStart, int? DayStart, int? DatePosition, int? catoid)
        {
            if (catoid == null)
            {
                return null;
            }

            DateTime Start = DateTime.Today;
            int? Month = MonthStart,
                Day = DayStart;
            try
            {
                Start = new DateTime((int)Year, (int)Month, (int)Day);
            }
            catch
            {
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        Day--;
                        Start = new DateTime((int)Year, (int)Month, (int)Day);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            IList<DateTime> Dates = db.FieldNDVIJsons
                .Where(f => f.Date >= Start && f.Date.Year == Year)
                .Select(f => f.Date)
                .Distinct()
                .ToList();
            Dates = Dates.OrderBy(d => d.Date).ToList();
            DateTime Date = Dates[DatePosition == null ? 0 : (int)DatePosition];

            FieldNDVIJson fnj = db.FieldNDVIJsons.Where(f => f.catoid == catoid && f.Date == Date).FirstOrDefault();
            
            JavaScriptSerializer j = new JavaScriptSerializer();
            j.MaxJsonLength = int.MaxValue;
            object json = j.Deserialize(fnj.Json, typeof(object));
            var jsonResult = Json(json);
            
            return jsonResult;
        }
        
        public ActionResult GetSownAreaChartImage(int? Year, int? CATOId)
        {
            if (CATOId == null)
            {
                return null;
            }

            List<decimal> values = new List<decimal>();
            List<string> names = new List<string>();

            var fields = db.fields
                .Where(f => f.catoid == CATOId)
                .Select(f => f.gid)
                .ToList();

            var croprotations = db.CropRotations
                .Where(c => fields.Contains(c.fieldgid) && c.Year == Year)
                .ToList();

            CATO cato = db.CATOes.Where(c => c.Id == CATOId).FirstOrDefault();
            if(cato.CD == "00" && cato.EF == "00")
            {
                fields = db.fields
                    .Select(f => f.gid)
                    .ToList();
                croprotations = db.CropRotations
                    .Where(c => c.Year == Year)
                    .ToList();
            }

            foreach(CropRotation cr in croprotations)
            {
                if (!names.Contains(cr.CropRotationTypeId.ToString()))
                {
                    names.Add(cr.CropRotationTypeId.ToString());
                    values.Add(0);
                }
                for (int i = 0; i < names.Count(); i++)
                {
                    if(names[i] == cr.CropRotationTypeId.ToString())
                    {
                        field field_ = db.fields.Where(f => f.gid == cr.fieldgid).FirstOrDefault();
                        values[i] += field_.area;
                        break;
                    }
                }
            }

            for(int j = 0; j < names.Count(); j++)
            {
                for (int i = 0; i < names.Count()-1; i++)
                {
                    if(Convert.ToInt32(names[i]) > Convert.ToInt32(names[i+1]))
                    {
                        string sbufer = names[i];
                        names[i] = names[i + 1];
                        names[i + 1] = sbufer;
                        decimal dbufer = values[i];
                        values[i] = values[i + 1];
                        values[i + 1] = dbufer;
                    }
                }
            }

            for (int i = 0; i < names.Count(); i++)
            {
                int? name = Convert.ToInt32(names[i]);
                names[i] = db.CropRotationTypes.Where(c => c.Id == name).FirstOrDefault().NameRU;
            }

            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = values[i] / 10000;
            }

            string title = cato.Name + " (" + Year.ToString() + ")";

            var chart_SownArea = new Chart(width: 400, height: 300, theme: ChartTheme.Green)
            .AddTitle(title)
            .AddSeries(
                chartType: "Column",
                xValue: names,
                yValues: values,
                name: GeoServer.Resources.GrainProduction.SownAreaha)
            .AddLegend();

            return File(chart_SownArea.ToWebImage().GetBytes(), "image/jpeg");
        }
    }
}