using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ExaminationManagement
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //登录星空动态
            bundles.Add(new ScriptBundle("~/Scripts/StarrySky").Include("~/Scripts/Account/EasePack.min.js","~/Scripts/Account/TweenLite.min.js","~/Scripts/Account/StarrySky.js"));
            //font-awesome
            bundles.Add(new StyleBundle("~/Content/font-awesome").Include("~/Content/font-awesome.min.css"));
            //进度条
            bundles.Add(new StyleBundle("~/Content/Loadding").Include("~/Content/Shared/pace-theme-minimal.css"));
            bundles.Add(new ScriptBundle("~/Scripts/Loadding").Include("~/Scripts/pace.min.js"));
            //jQuery
            bundles.Add(new ScriptBundle("~/Scripts/jQuery").Include("~/Scripts/jquery-3.3.1.min.js"));
            //bootstrap
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.min.css"));
            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            //bootstrap table without x-editable
            bundles.Add(new StyleBundle("~/Content/bootstrap-table-noedit").Include("~/Content/bootstrap-table.min.css"));
            bundles.Add(new ScriptBundle("~/Scripts/bootstrap-table-noedit").Include("~/Scripts/bootstrap-table.min.js", "~/Scripts/bootstrap-table-zh-CN.min.js"));
            //bootstrap table
            bundles.Add(new StyleBundle("~/Content/bootstrap-table").Include("~/Content/bootstrap-table.min.css", "~/Content/bootstrap3-editable/css/bootstrap-editable.css"));
            bundles.Add(new ScriptBundle("~/Scripts/bootstrap-table").Include(
                "~/Scripts/bootstrap3-editable/js/bootstrap-editable.min.js",
                "~/Scripts/bootstrap-table.min.js",
                "~/Scripts/bootstrap-table-zh-CN.min.js",
                "~/Scripts/bootstrap-table-editable.js"
                ));
        }
    }
}