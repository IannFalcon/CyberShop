using System.Web;
using System.Web.Optimization;

namespace AppProyectoEFSRTCyberShop
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/complementos").Include(
                        "~/Scripts/fontawesome/all.min.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/datatables.responsive.js",
                        "~/Scripts/loadingoverlay/loadingoverlay.min.js",
                        "~/Scripts/sweetalert2.all.min.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery-ui-1.13.2*",
                        "~/Scripts/scripts.js"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css",
                        "~/Content/DataTables/css/jquery.dataTables.css",
                        "~/Content/DataTables/css/responsive.dataTables.css",
                        "~/Content/themes/base/jquery-ui*",
                        "~/Content/sweetalert2.min.css"));
        }
    }
}