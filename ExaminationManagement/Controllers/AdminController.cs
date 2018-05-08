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
        
        public ActionResult AddMajor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMajor(Models.WebModels.Course[] courses)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddCourse(courses);
            if (flag)
                return Content("success");
            return Content("failure");
        }
    }
}