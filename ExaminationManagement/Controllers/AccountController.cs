using ExaminationManagement.Models;
using ExaminationManagement.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExaminationManagement.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            SQLManager manager = new SQLManager();
            RoleType type = manager.CheckUser(username, password);
            switch (type)
            {
                case RoleType.Admin:
                    
                case RoleType.Teacher:
                    
                case RoleType.Student:
                    
                default:
                    return Content(type.ToString());
            }
        }
    }
}