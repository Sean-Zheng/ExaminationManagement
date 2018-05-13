using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExaminationManagement.Models;

namespace ExaminationManagement.Controllers
{
    public class AnalysisController : Controller
    {
        public ActionResult Course(int id)
        {
            var scores = new SQLManager().Course(id);
            var group = from item in scores group item.Score by item.Name into g select g;
            List<ResultModel> models = new List<ResultModel>();
            foreach (var item in group)
            {
                ResultModel model = new ResultModel
                {
                    name = item.Key
                };
                model.data.Add(item.Min());
                model.data.Add(item.Average());
                model.data.Add(item.Max());
                models.Add(model);
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Class(int c,int m,int e)
        {
            var scores = new SQLManager().Class(c, m, e);
            var group = from item in scores group item.Score by item.Name into g select g;
            List<ResultModel> models = new List<ResultModel>();
            foreach (var item in group)
            {
                ResultModel model = new ResultModel
                {
                    name = item.Key
                };
                model.data.Add(item.Min());
                model.data.Add(item.Average());
                model.data.Add(item.Max());
                models.Add(model);
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Teacher(string id)
        {
            var scores = new SQLManager().Teacher(id);
            var group = from item in scores group item.Score by item.Name into g select g;
            List<ResultModel> models = new List<ResultModel>();
            foreach (var item in group)
            {
                ResultModel model = new ResultModel
                {
                    name = item.Key
                };
                model.data.Add(item.Min());
                model.data.Add(item.Average());
                model.data.Add(item.Max());
                models.Add(model);
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }
    }
}