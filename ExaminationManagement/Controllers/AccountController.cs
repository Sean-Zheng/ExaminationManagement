using ExaminationManagement.Models;
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
            return null;
        }
        public string Test(string username,string password)
        {
            SQLManager manager = new SQLManager();
            return manager.CheckUser(username, password).ToString();
        }
    }
}