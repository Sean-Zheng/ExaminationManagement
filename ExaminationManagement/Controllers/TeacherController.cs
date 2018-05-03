using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExaminationManagement.Models;

namespace ExaminationManagement.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 上传学生成绩
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAchievement(HttpPostedFileBase file)
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Temp")
                + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + file.FileName;
            file.SaveAs(fileName);

            if (System.IO.File.Exists(fileName))
                return Json(null);
            return null;
        }
        /// <summary>
        /// 上传学生信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadInformation(HttpPostedFileBase file)
        {
            return null;
        }
        /// <summary>
        /// 下载学生成绩模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoadAchievement()
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Templates/学生成绩模板.xlsx");
            return File(fileName, MimeMapping.GetMimeMapping(fileName), "学生成绩模板.xlsx");
        }
        /// <summary>
        /// 下载学生信息模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoadInformation()
        {
            string fileName = HttpContext.Server.MapPath("~/Resources/Templates/学生信息模板.xlsx");
            return File(fileName, MimeMapping.GetMimeMapping(fileName), "学生信息模板.xlsx");
        }
    }
}