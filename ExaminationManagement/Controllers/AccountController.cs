using ExaminationManagement.Models;
using ExaminationManagement.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
                    HttpCookie adminCookie = GetCookie(username, "Admin");
                    Response.Cookies.Add(adminCookie);
                    break;
                case RoleType.Teacher:
                    HttpCookie teacherCookie = GetCookie(username, "Teacher");
                    Response.Cookies.Add(teacherCookie);
                    break;
                case RoleType.Student:
                    HttpCookie studentCookie = GetCookie(username, "Student");
                    Response.Cookies.Add(studentCookie);
                    break;
                default:
                    break;
            }
            return Content(type.ToString());
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        public ActionResult Test(string id)
        {
            if (id == "1")
                return RedirectToAction("Index", "Admin");
            else
                return new EmptyResult();
            //return RedirectToActionPermanent("Index", "Admin");
            //return Redirect("/Admin/Index");
        }
        private HttpCookie GetCookie(string username,string userType)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(60),
                false,
                userType,
                FormsAuthentication.FormsCookiePath
                );
            // 加密票证
            string encTicket = FormsAuthentication.Encrypt(ticket);
            // 创建cookie
            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }
    }
}