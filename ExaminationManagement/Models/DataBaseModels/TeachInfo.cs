using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.DataBaseModels
{
    /// <summary>
    /// 教师信息
    /// </summary>
    public class TeachInfo:Person
    {
        /// <summary>
        /// 教师工号
        /// </summary>
        public string Tea_id { get; set; }
    }
}