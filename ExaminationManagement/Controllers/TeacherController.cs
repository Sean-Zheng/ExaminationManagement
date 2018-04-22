using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExaminationManagement.Models;

namespace ExaminationManagement.Controllers
{
    public class TeacherController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 下载学生成绩模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Achievement()
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Templates/学生成绩模板.xlsx");
            return File(fileName, MimeMapping.GetMimeMapping(fileName), "学生成绩模板.xlsx");
        }
        /// <summary>
        /// 下载学生信息模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Information()
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Templates/学生信息模板.xlsx");
            return File(fileName, MimeMapping.GetMimeMapping(fileName), "学生信息模板.xlsx");
        }
    }
}