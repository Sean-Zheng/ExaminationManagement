using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ExaminationManagement.Models;

namespace ExaminationManagement.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.userName = User.Identity.Name;
            return View();
        }
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <returns></returns>
        public ActionResult Course()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Course(Models.WebModels.Course[] courses)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddCourse(courses);
            if (flag)
                return Content("success");
            return Content("failure");
        }
        /// <summary>
        /// 课程信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            SQLManager manager = new SQLManager();
            IEnumerable<Models.DataBaseModels.Course> courses = manager.GetCourses();
            //Models.DataBaseModels.Course course = new Models.DataBaseModels.Course();
            //course.CourseName = "sd";
            //List<Models.DataBaseModels.Course> courses = new List<Models.DataBaseModels.Course>();
            //courses.Add(course);
            return View(courses);
        }
        public ActionResult CourseDelete(int id)
        {
            SQLManager manager = new SQLManager();
            manager.DeleteCourse(id);
            return RedirectToAction("Message");
        }
    }
}