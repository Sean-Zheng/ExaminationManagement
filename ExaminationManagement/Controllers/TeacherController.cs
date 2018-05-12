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
        [TeacherName]
        public ActionResult Student()
        {
            SQLManager manager = new SQLManager();
            var student = manager.GetStudents(User.Identity.Name);
            return View(student);
        }
        [HttpGet]
        public ActionResult MajorList()
        {
            SQLManager manager = new SQLManager();
            var options = manager.GetMajors();
            return Json(options, JsonRequestBehavior.AllowGet);
        }
        [TeacherName]
        public ActionResult Grade()
        {
            SQLManager manager = new SQLManager();
            var grades = manager.GetGrades(User.Identity.Name);
            return View(grades);
        }
        public ActionResult GradeFor(int id)
        {
            SQLManager manager = new SQLManager();
            var grades = manager.GetGrades(User.Identity.Name, id);
            return View("Grade", grades);
        }
        public ActionResult EditGrade(Models.WebModels.Grade grade)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateGrade(grade);
            return new EmptyResult();
        }
        public ActionResult CourseList()
        {
            SQLManager manager = new SQLManager();
            return Json(manager.CourseList(User.Identity.Name), JsonRequestBehavior.AllowGet);
        }
        #region 
        /// <summary>
        /// 上传学生成绩
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAchievement(HttpPostedFileBase file,int courseId)
        {
            string path = null;
            try
            {
                if (file == null)
                    return Json(new { exists = false });
                string fileName = HttpContext.Server.MapPath("~/Resources/Temp/")
                    + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + System.IO.Path.GetFileName(file.FileName);
                file.SaveAs(fileName);
                if (!System.IO.File.Exists(fileName))
                    return Json(new { exists = false });
                path = fileName;
                ExcelManager excel = new ExcelManager(fileName);
                if (!excel.CheackAchievementTemplates())
                    return Json(new { exists = true, legal = false });

                SQLManager manager = new SQLManager();
                var grades = excel.GetAchievement();

                bool flag = manager.AddGrades(grades, courseId);
                if (flag)
                    return Json(new { exists = true, legal = true, success = true });
                return Json(new { exists = true, legal = true, success = false });
            }
            finally
            {
                if (path != null)
                    System.IO.File.Delete(path);
            }
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