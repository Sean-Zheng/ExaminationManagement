using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ExaminationManagement.Models;

namespace ExaminationManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [AdminId]
        public ActionResult Index()
        {
            return View();
        }
        #region 专业
        [AdminId]
        public ActionResult Major()
        {
            return View();
        }
        /// <summary>
        /// 添加专业
        /// </summary>
        /// <param name="majors"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Major(Models.WebModels.Major[] majors)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddMajors(majors);
            if (flag)
                return Content("success");
            return Content("failure");
        }
        /// <summary>
        /// 专业信息
        /// </summary>
        /// <returns></returns>
        [AdminId]
        public ActionResult MajorMessage()
        {
            SQLManager manager = new SQLManager();
            var majors = manager.GetMajorList();
            return View(majors);
        }
        /// <summary>
        /// 修改专业
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="majorName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditMajor(int majorId, Models.WebModels.Major major)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateMajors(majorId, major);
            return new EmptyResult();
        }
        /// <summary>
        /// 删除专业
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public ActionResult DeleteMajor(int majorId)
        {
            SQLManager manager = new SQLManager();
            manager.DeleteMajors(majorId);
            return RedirectToAction("MajorMessage");
        }
        /// <summary>
        /// 获取专业列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MajorList()
        {
            SQLManager manager = new SQLManager();
            var options = manager.GetMajors();
            return Json(options, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 教师
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <returns></returns>
        [AdminId]
        public ActionResult Teacher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Teacher(Models.WebModels.Teacher[] teachers)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddTeachers(teachers);
            if (flag)
                return Content("success");
            return Content("failure");
        }
        [AdminId]
        public ActionResult TeacherMessage()
        {
            SQLManager manager = new SQLManager();
            var teachers = manager.GetTeachInfoList();
            return View(teachers);
        }
        public ActionResult DeleteTeacher(string id)
        {
            SQLManager manager = new SQLManager();
            manager.DeleteTacher(id);
            return RedirectToAction("TeacherMessage");
        }
        public ActionResult EditTeacher(string oldTeaId,Models.WebModels.Teacher teacher)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateTeacher(oldTeaId, teacher);
            return new EmptyResult();
        }
        [HttpGet]
        public ActionResult TeacherList()
        {
            SQLManager manager = new SQLManager();
            var teacher = manager.GetTeachers();
            return Json(teacher, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 学生
        [AdminId]
        public ActionResult StudentMessage()
        {
            SQLManager manager = new SQLManager();
            var students = manager.GetStudents();
            return View(students);
        }
        [AdminId]
        public ActionResult StudentMessageWith(int id)
        {
            if (id == 0)
                return RedirectToAction("StudentMessage");
            SQLManager manager = new SQLManager();
            var students = manager.GetStudents(id);
            return View("StudentMessage", students);
        }
        public ActionResult EditStudent(string oleStuId,Models.WebModels.Student student)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateStudent(oleStuId, student);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult DeleteStudents(string[] ids)
        {
            SQLManager manager = new SQLManager();
            manager.DeleteStudents(ids);
            return RedirectToAction("StudentMessage");
        }
        public ActionResult InsertStudent(Models.WebModels.Student student)
        {
            SQLManager manager = new SQLManager();
            manager.AddStudent(student);
            return new EmptyResult();
        }
        public ActionResult GetStudentYears()
        {
            SQLManager manager = new SQLManager();
            var list = manager.GetYearList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #region 
        /// <summary>
        /// 上传学生信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadInformation(HttpPostedFileBase file)
        {
            string path = null;
            try
            {
                if (file == null)
                    return Json(new { exists = false });
                string fileName = HttpContext.Server.MapPath("~/Resources/Temp/")
                    + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + file.FileName;
                file.SaveAs(fileName);
                if (!System.IO.File.Exists(fileName))
                    return Json(new { exists = false });
                path = fileName;
                ExcelManager excel = new ExcelManager(fileName);
                if (!excel.CheackInformationTemplates())
                    return Json(new { exists = true, legal = false });
                SQLManager manager = new SQLManager();
                var informations = excel.GetInformation();
                List<string> majorNames = new List<string>();
                foreach (var item in informations)
                    majorNames.Add(item.Major);
                if (!manager.CheckMajorExist(majorNames.ToArray()))
                    return Json(new { exists = true, legal = false });
                bool flag = manager.AddStudents(informations);
                if (flag)
                    return Json(new { exists = true, legal = true, success = true });
                return Json(new { exists = true, legal = true, success = false });
            }
            finally
            {
                if (path != null)
                    System.IO.File.Delete(path);
            }
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
        #endregion
        #endregion

        #region 课程
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <returns></returns>
        [AdminId]
        public ActionResult Course()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Course(Models.WebModels.Course[] courses)
        {
            SQLManager manager = new SQLManager();
            bool flag = manager.AddCourse(courses);
            if (flag)
                return Content("success");
            return Content("failure");
        }
        /// <summary>
        /// 课程信息
        /// </summary>
        /// <returns></returns>
        [AdminId]
        public ActionResult CourseMessage()
        {
            SQLManager manager = new SQLManager();
            IEnumerable<Models.DataBaseModels.Course> courses = manager.GetCourses();
            return View(courses);
        }
        public ActionResult CourseDelete(int id)
        {
            SQLManager manager = new SQLManager();
            manager.DeleteCourse(id);
            return RedirectToAction("CourseMessage");
        }
        public ActionResult EditCourse(Models.DataBaseModels.Course course)
        {
            SQLManager manager = new SQLManager();
            manager.UpdateCourse(course);
            return new EmptyResult();
        }
        #endregion
    }

    /// <summary>
    /// 管理员用户名过滤器
    /// </summary>
    public class AdminIdAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.userName = filterContext.HttpContext.User.Identity.Name;
        }
    }
}