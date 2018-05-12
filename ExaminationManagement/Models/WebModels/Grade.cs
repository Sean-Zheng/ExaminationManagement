using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.WebModels
{
    public class Grade
    {
        public int Id { get; set; }
        public double? DailyWork { get; set; }
        public double? MidExam { get; set; }
        public double? FinalExam { get; set; }
        public double? TotalRemark { get; set; }
    }
}