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
            //星空包
            bundles.Add(new StyleBundle("~/Scripts/StarrySky").Include("~/Scripts/Account/EasePack.min.js","~/Scripts/Account/TweenLite.min.js","~/Scripts/Account/StarrySky.js"));
        }
    }
}