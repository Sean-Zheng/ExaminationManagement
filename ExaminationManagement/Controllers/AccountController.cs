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
                    break;
                case RoleType.Teacher:
                    break;
                case RoleType.Student:
                    break;
                case RoleType.NotFound:
                    break;
                default:
                    break;
            }
            return null;
        }
        public string Test(string username,string password)
        {
            SQLManager manager = new SQLManager();
            return manager.CheckUser(username, password).ToString();
        }
    }
}