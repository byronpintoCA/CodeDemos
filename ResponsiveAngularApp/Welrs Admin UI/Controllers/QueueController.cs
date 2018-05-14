using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Welrs_Admin_UI.Controllers
{
    public class QueueController : Controller
    {
        // GET: Queue
        //[HttpGet]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            ViewBag.MainPageId = 2;
            return View("QueueIndex");
        }
    }
}