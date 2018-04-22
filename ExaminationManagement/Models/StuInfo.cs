using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models
{
    public class StuInfo:Person
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string Stu_id { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public int Enroll_year { get; set; }
        /// <summary>
        /// 已获学分
        /// </summary>
        public float Credit_got { get; set; }
        /// <summary>
        /// 学分要求
        /// </summary>
        public float Credit_need { get; set; }
    }
}