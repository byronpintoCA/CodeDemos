using System;
using System.Web.Optimization;

namespace Welrs_Admin_UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/JqueryBootstrapDependencies").Include(
                "~/ScriptsFontsCSS/js/jquery.js",
                "~/ScriptsFontsCSS/js/bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/HCAssignmentDependencies").Include(
                "~/ScriptsFontsCSS/js/jquery.js",
                "~/ScriptsFontsCSS/js/bootstrap.js",
                "~/ScriptsFontsCSS/js/bootstrap-treeview.js",
                "~/ScriptsFontsCSS/js/TreeViewHelper.js",
                "~/ScriptsFontsCSS/js/angular.js",
                "~/ScriptsFontsCSS/js/jquery-ui.js"
                //"~/ScriptsFontsCSS/js/HCAssignment-angular.js",
                //"~/ScriptsFontsCSS/js/HCAssignment.js"
                ));

          //BundleTable.EnableOptimizations = true;
        }
    }
}