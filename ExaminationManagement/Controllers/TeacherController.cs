﻿using System;
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
        /// 上传学生成绩
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadAchievement(HttpPostedFileBase file)
        {
            
            return null;
        }
        /// <summary>
        /// 上传学生信息
        /// </summary>
        /// <returns></returns>
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