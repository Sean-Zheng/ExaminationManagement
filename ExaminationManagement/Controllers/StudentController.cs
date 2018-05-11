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
        [StudentName]
        public ActionResult Index()
        {
            SQLManager manager = new SQLManager();
            StuInfo stuInf = manager.GetStuInfo(User.Identity.Name);
            return View(stuInf);
        }
    }

    /// <summary>
    /// 学生姓名过滤器
    /// </summary>
    public class StudentNameAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SQLManager manager = new SQLManager();
            filterContext.Controller.ViewBag.userName = manager.SelectTeacherName(filterContext.HttpContext.User.Identity.Name);
        }
    }
}