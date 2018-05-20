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
        /// <summary>
        /// 登录界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View("Index");
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldpwd"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string oldpwd,string newpwd)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.ChangeUserPwd(User.Identity.Name, oldpwd, newpwd);
            if (flag)
                return Content("success");
            return Content("failure");
        }

        public ActionResult Changepwd()
        {
            return PartialView();
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="userType">类型</param>
        /// <returns></returns>
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