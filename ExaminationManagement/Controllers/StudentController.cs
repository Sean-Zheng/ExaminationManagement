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
        [HttpPost]
        [Authorize]
        public ActionResult Change(Person person)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateUserInfo(person, 2, User.Identity.Name);
            return new EmptyResult();
        }
        public ActionResult Evaluate(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Evaluate(int id,string content)
        {
            SQLManager manager = new SQLManager();
            if (manager.AddEvaluate(id,content))
                return Content("success");
            return Content("failure");
        }
        //[HttpGet]
        //public ActionResult MajorList()
        //{
        //    SQLManager manager = new SQLManager();
        //    var options = manager.GetMajorList();
        //    return Json(options, JsonRequestBehavior.AllowGet);
        //}

        [StudentName]
        public ActionResult Personal()
        {
            SQLManager manager = new SQLManager();
            StuInfo info = manager.GetStuInfo(User.Identity.Name);
            return View(info);
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