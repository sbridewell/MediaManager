using System.Web;
using System.Web.Optimization;

namespace MediaManager.Mvc
{
    /// <summary>
    /// Contains the method for bundling up disparate style sheets and javascript files
    /// into a smaller number of files in logical web server locations independent of
    /// their file system locations.
    /// </summary>
    /// <remarks>
    /// For more information on bundling, visit 
    /// <see href="https://go.microsoft.com/fwlink/?LinkId=301862"/>.
    /// </remarks>
    public class BundleConfig
    {
        /// <summary>
        /// Bundles up style sheets and javascript files.
        /// </summary>
        /// <param name="bundles">
        /// Supplied by the runtime.
        /// Add bundles to this parameter using the Add method.
        /// </param>
        /// <remarks>
        /// For more information on bundling, visit 
        /// <see href="https://go.microsoft.com/fwlink/?LinkId=301862"/>.
        /// </remarks>
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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css", 
                      "~/Content/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css")
                .Include("~/Content/themes/base/jquery-ui.css"));
        }
    }
}
