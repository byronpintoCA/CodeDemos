using SearchService;
using Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace VehicleLookupApi.Controllers
{
    [CORSHeader]
    public class ClientController : Controller
    {
        /// <summary>
        /// Displays Jquery Client
        /// </summary>
        /// <returns></returns>
        public ActionResult JQuery()
        {
            return View();
        }

        /// <summary>
        /// Displays Angular Client
        /// </summary>
        /// <returns></returns>
        public ActionResult Angular()
        {
            return View();
        }

        /// <summary>
        /// Displays Demo Features
        /// </summary>
        /// <returns></returns>
        public ActionResult Features()
        {
            return View();
        }

        [System.Web.Http.HttpGet]
        public JsonResult Search(String searchCriteria)
        {
            var btree = (BTree<Vehicle>)HttpContext.Application[ApplicationKeys.ModelSearchTree];
            var suggestionList = btree.Children(searchCriteria);

            var a = new { total = suggestionList.Count, results = suggestionList.Select(vh => vh.Model).ToList() };
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This generates a sample error and demonstrates the use of Elmah for error logging and administration
        /// URL : /elmah.axd. 
        /// Elmah access can be restricted to authorized users e.g  https://stackoverflow.com/questions/6778881/how-can-i-restrict-remote-access-to-elmah
        /// </summary>
        [System.Web.Http.HttpGet]
        public virtual void Error(String id)
        {
            throw new Exception("Unhandled Exception");
        }

        [System.Web.Http.HttpGet]
        public virtual ActionResult SearchPerformance()
        {
            return View();
        }

    }
}