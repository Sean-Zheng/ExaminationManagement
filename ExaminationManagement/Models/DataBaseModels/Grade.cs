using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.DataBaseModels
{
    public class Grade
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Term { get; set; }
        public double? DailyWork { get; set; }
        public double? MidExam { get; set; }
        public double? FinalExam { get; set; }
        public double? TotalRemark { get; set; }
        public int Status { get; set; }
    }
    public class GradeForStudent
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Term { get; set; }
        public double? TotalRemark { get; set; }
        public int Status { get; set; }
        public string TeacherName { get; set; }
    }
}