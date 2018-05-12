using ExaminationManagement.Models;
using ExaminationManagement.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExaminationManagement.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        [StudentName]
        public ActionResult Index()
        {
            SQLManager manager = new SQLManager();
            var grades = manager.GetStudentGrades(User.Identity.Name, 0);
            return View(grades);
        }
        [StudentName]
        public ActionResult GradeBy(int id)
        {
            SQLManager manager = new SQLManager();
            var grades = manager.GetStudentGrades(User.Identity.Name, id);
            return View("Index",grades);
        }

        public ActionResult CourseList()
        {
            SQLManager manager = new SQLManager();
            IEnumerable<Course> courses = manager.GetCourses();
            return Json(courses, JsonRequestBehavior.AllowGet);
        }
    }

    /// <summary>
    /// 学生姓名过滤器
    /// </summary>
    public class StudentNameAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SQLManager manager = new SQLManager();
            filterContext.Controller.ViewBag.userName = manager.SelectStudentName(filterContext.HttpContext.User.Identity.Name);
        }
    }
}