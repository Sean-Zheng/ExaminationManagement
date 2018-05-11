using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.WebModels
{
    public class Student
    {
        public string StuId { get; set; }
        public string Name { get; set; }
        public int MajorId { get; set; }
        public int EnrollYear { get; set; }
        public int ClassNumber { get; set; }
        public Gender Gender { get; set; }
    }
}