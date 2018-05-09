using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.WebModels
{
    public class Teacher
    {
        public string Tea_id { get; set; }
        public string TeaName { get; set; }
        public string Passwd { get; set; }
        public int MajorId { get; set; }
    }
}