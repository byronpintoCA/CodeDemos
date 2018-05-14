using AdminUI.Common;
using AdminUI.DataProvider;
using AdminUI.Models;
using AdminUI.Models.HCAssignment;
using AdminUI.Models.Queue;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Welrs_Admin_UI.Controllers
{

   // [WelrsAuthorize] 
    public class HCAssignmentController : Controller
    {
        //[OutputCache(Duration = 86400 ,Location=OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            ViewBag.MainPageId = 3;
            ViewBag.ShowControlSection = true;
            return View("HCAIndex");
        }

        [HttpGet]
        [ValidateHeaderAntiForgeryToken]
        public ActionResult FailedQueItems()
        {
            var prov = HCAssignmentDataProviderFactory.GetProvider();

            List<MSH> msh = prov.GetMSH();

            var tree = TreeViewHelper.Transform(msh);

            var ret = new TreeViewData()
            {
                MSHData = msh,
                TreeData = tree,
                AssignCodes = prov.GetAssignmentCodes(),
                TreeConfig = new TreeConfig()
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateHeaderAntiForgeryToken]
        public String HL7Data(int msh_id)
        {
            var prov = HCAssignmentDataProviderFactory.GetProvider();
            return prov.GetHL7Message(msh_id);
        }

        [HttpPost]
        [ValidateHeaderAntiForgeryToken]
        public ActionResult Save(int MSH_ID, string Note, List<ChangeSetData> ChangeSet)
        {
            var prov = HCAssignmentDataProviderFactory.GetProvider();
            String errorMessage = "";
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            var success = prov.Save(username, MSH_ID, Note == null ? "" : Note, ChangeSet, out errorMessage);
            return Json(new ResponseStatus()
            {
                Success = success == true ? ResponseStatus.ApiResponseStatus.Success : ResponseStatus.ApiResponseStatus.Failure,
                ErrorMessage = errorMessage
            });
        }
    }
}