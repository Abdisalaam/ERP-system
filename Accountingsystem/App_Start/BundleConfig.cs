using System.Web;
using System.Web.Optimization;

namespace Accountingsystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            ////Create bundel for jQueryUI  
            ////js  
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //          "~/Scripts/jquery-ui-{version}.js"));
            ////css  
            //bundles.Add(new StyleBundle("~/Content/cssjqryUi").Include(
            //       "~/Content/jquery-ui.css"));

            ////scripts
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //        "~/Scripts/bootstrap.js",
            //        "~/Scripts/moment.min.js",
            //        "~/scripts/bootstrap-datetimepicker.js",
            //        "~/Scripts/respond.js",
            //        "~/Scripts/dataTables.bootstrap.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
            //    "~/Scripts/plugins.js"
            //    ));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/bootstrap-datetimepicker.css",
            //          "~/Content/site.css",
            //          "~/Content/dataTables.bootstrap.min.css"));


            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //    "~/Scripts/jquery-{version}.js",
            //    "~/Scripts/jquery.validate.min.js",
            //    "~/Scripts/jquery.validate.unobtrusive.min.js"));
            //BundleTable.EnableOptimizations = true;

        }
    }
}
