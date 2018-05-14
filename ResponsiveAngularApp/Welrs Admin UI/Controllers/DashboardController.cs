using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Welrs_Admin_UI.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        //[HttpGet]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            ViewBag.MainPageId = 1;
            return View("DashboardIndex");
        }
    }
}