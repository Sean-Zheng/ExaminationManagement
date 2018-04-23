using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models
{
    public class User
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}