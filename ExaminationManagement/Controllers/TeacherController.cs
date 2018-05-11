using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExaminationManagement.Models;

namespace ExaminationManagement.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        [TeacherName]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }


        #region 
        /// <summary>
        /// 上传学生成绩
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAchievement(HttpPostedFileBase file)
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Temp")
                + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + file.FileName;
            file.SaveAs(fileName);

            if (System.IO.File.Exists(fileName))
                return Json(null);
            return null;
        }
        /// <summary>
        /// 下载学生成绩模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoadAchievement()
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Templates/学生成绩模板.xlsx");
            return File(fileName, MimeMapping.GetMimeMapping(fileName), "学生成绩模板.xlsx");
        }
        #endregion
    }

    /// <summary>
    /// 教师姓名过滤器
    /// </summary>
    public class TeacherNameAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SQLManager manager = new SQLManager();
            filterContext.Controller.ViewBag.userName = manager.SelectTeacherName(filterContext.HttpContext.User.Identity.Name);
        }
    }
}