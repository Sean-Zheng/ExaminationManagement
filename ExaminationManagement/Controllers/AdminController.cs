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
        public ActionResult Major()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Major(string[] majors)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddMajors(majors);
            if (flag)
                return Content("success");
            return Content("failure");
        }
        public ActionResult MajorMessage()
        {
            SQLManager manager = new SQLManager();
            var majors = manager.GetMajors();
            return View(majors);
        }
        [HttpPost]
        public ActionResult EditMajor(int majorId,string majorName)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateMajors(majorId, majorName);
            return new EmptyResult();
        }
        public ActionResult DeleteMajor(int majorId)
        {
            SQLManager manager = new SQLManager();
            manager.DeleteMajors(majorId);
            return RedirectToAction("MajorMessage");
        }
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <returns></returns>
        public ActionResult Teacher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Teacher(Models.WebModels.Teacher[] teachers)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddTeachers(teachers);
            if (flag)
                return Content("success");
            return Content("failure");
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