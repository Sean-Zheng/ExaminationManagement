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
        //public string Test()
        //{
        //    //HttpContext.Current.Server.MapPath("~/Resources/Templates/学生成绩模板.xlsx");
        //    return MimeMapping.GetMimeMapping(PathInfo.File_Achievement);
        //}
    }
}