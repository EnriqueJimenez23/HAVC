using System.Web.Optimization;

namespace CsWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/assets/cssplantilla").Include(
              "~/Content/assets/css/bootstrap.css",
              "~/Content/assets/css/font-awesome.css",
              "~/Content/assets/css/FuentesPag.css",
              "~/Content/assets/css/jquery-ui.css",
              "~/Content/assets/css/ui.jqgrid.css",
              "~/Content/assets/css/ace-fonts.css",
              "~/Content/assets/css/ace.css",
              "~/Content/assets/css/chosen.css",
              "~/Content/assets/css/datepicker.css",
              "~/Content/assets/iCheck/custom.css"

              ));
            bundles.Add(new StyleBundle("~/Content/assets/cssbootstrap").Include(
             "~/Content/assets/css/bootstrap.css"
             //"~/Content/assets/css/font-awesome.css",
             //"~/Content/assets/css/FuentesPag.css",
             //"~/Content/assets/css/jquery-ui.css",
             //"~/Content/assets/css/ui.jqgrid.css",
             //"~/Content/assets/css/ace-fonts.css",
             //"~/Content/assets/css/ace.css",
             //"~/Content/assets/css/chosen.css",
             //"~/Content/assets/css/datepicker.css",
             //"~/Content/assets/iCheck/custom.css"
             ));
            bundles.Add(new StyleBundle("~/Content/csspulep").Include(
                "~/Content/rc.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ace").Include(
                "~/Content/assets/js/ace-extra.js",
                "~/Content/assets/js/html5shiv.js",
                "~/Content/assets/js/respond.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.5.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/scriptsplantilla").Include(
                "~/Content/assets/js/bootstrap.js",
                "~/Content/assets/js/jqGrid/jquery.jqGrid.src.js",
                "~/Content/assets/js/jqGrid/i18n/grid.locale-es.js",
                "~/Content/assets/js/ace/elements.scroller.js",
                "~/Content/assets/js/ace/elements.colorpicker.js",
                "~/Content/assets/js/ace/elements.fileinput.js",
                "~/Content/assets/js/ace/elements.typeahead.js",
                "~/Content/assets/js/ace/elements.typeahead.js",
                "~/Content/assets/js/ace/elements.wysiwyg.js",
                "~/Content/assets/js/ace/elements.spinner.js",
                "~/Content/assets/js/ace/elements.treeview.js",
                "~/Content/assets/js/ace/elements.wizard.js",
                "~/Content/assets/js/ace/elements.aside.js",
                "~/Content/assets/js/ace/ace.js",
                "~/Content/assets/js/ace/ace.ajax-content.js",
                "~/Content/assets/js/ace/ace.touch-drag.js",
                "~/Content/assets/js/ace/ace.sidebar.js",
                "~/Content/assets/js/ace/ace.sidebar-scroll-1.js",
                "~/Content/assets/js/ace/ace.submenu-hover.js",
                "~/Content/assets/js/ace/ace.settings.js",
                "~/Content/assets/js/ace/ace.settings-rtl.js",
                "~/Content/assets/js/ace/ace.settings-skin.js",
                "~/Content/assets/js/ace/ace.widget-on-reload.js",
                "~/Content/assets/js/ace/ace.searchbox-autocomplete.js",
                "~/Content/assets/js/autoNumeric.js",
                "~/Content/assets/js/date-time/bootstrap-datepicker.js",
                "~/Content/assets/js/icheck.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scriptspulep").Include(
              "~/Scripts/PULEP.js", "~/Scripts/jquery.rwdImageMaps.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
              "~/Content/assets/js/chosen.jquery.js"));
            bundles.Add(new ScriptBundle("~/chartJs").Include(
                     "~/Scripts/Chart.min.js"));
        }
    }
}
