﻿using System.Web.Optimization;

namespace AdventOne {
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
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/site.css",
                      "~/Content/themes/base/jquery-ui.css"
                      ));



            bundles.Add(new ScriptBundle("~/bundles/mce_readonly_scripts").Include(
                "~/Scripts/tinymce/tiny_mce.js",
                "~/Scripts/mce_readonly.js"
                ));
            
            bundles.Add(new ScriptBundle("~/bundles/mce_editable_scripts").Include(
                "~/Scripts/tinymce/tiny_mce.js",
                "~/Scripts/mce_editable.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                "~/Scripts/moment*",
                "~/Scripts/bootstrap-datetimepicker*"
                ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.1.js"));


        }
    }
}
