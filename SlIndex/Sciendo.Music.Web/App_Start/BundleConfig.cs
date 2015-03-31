using System.Web.Optimization;

namespace Sciendo.Music.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/autocomplete").Include(
                "~/Content/themes/base/all.css",
                      "~/Content/themes/base/autocomplete.css"));
            bundles.Add(new StyleBundle("~/Content/indexing").Include(
                            "~/Content/indexing/specific.css"));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
"~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
"~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/index").Include(
"~/Scripts/sciendo.music/index.viewmodel.js",
"~/Scripts/sciendo.music/index.autocomplete.js"));
            bundles.Add(new ScriptBundle("~/bundles/query").Include(
    "~/Scripts/sciendo.music/knockout*",
"~/Scripts/sciendo.music/query*"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
