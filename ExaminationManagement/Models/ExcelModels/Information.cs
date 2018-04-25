using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.ExcelModels
{
    /// <summary>
    /// Excel学生信息导入模板
    /// </summary>
    public class Information
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Namme { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Sex { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public int ClassNumber { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public int EnrollmentYear { get; set; }
    }
}