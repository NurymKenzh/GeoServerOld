using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoServer.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            ViewBag.Moderator = false;
            ViewBag.Admin = false;
            if (User.Identity.IsAuthenticated)
            {
                if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
                {
                    ViewBag.Admin = true;
                }
                if (UserManager.IsInRole(User.Identity.GetUserId(), "Moderator"))
                {
                    ViewBag.Moderator = true;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Text";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Text";

            return View();
        }
    }
}