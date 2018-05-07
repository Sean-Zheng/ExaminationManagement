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
        public ActionResult Index()
        {
            SQLManager manager = new SQLManager();
            StuInfo stuInf = manager.GetStuInfo(User.Identity.Name);
            return View(stuInf);
        }
    }
}