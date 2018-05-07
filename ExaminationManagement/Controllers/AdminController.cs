using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExaminationManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public AdminController()
        {
            ViewBag.userName = User.Identity.Name;
        }
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult AddTeacher()
        //{

        //}
        
    }
}