using System.Web;
using System.Web.Optimization;

namespace fst_Career_Portal_Dev
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",  
                      "~/Content/css/popuo.css",
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Script/jquery-2.2.3.min.js",
                "~/Script/jquery.magnific-popup.js",
                "~/Script/bootstrap.min.js"));
        }
    }
}
