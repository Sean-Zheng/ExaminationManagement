using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models
{
    public class ClassModel
    {
        public int MajorId { get; set; }
        public string MajorName { get; set; }
        public int Year { get; set; }
        public int ClassNumber { get; set; }
    }

    public class ClassSelect
    {
        public DataBaseModels.Major[] Majors { get; set; }
        public int[] Years { get; set; }
        public int[] Number { get; set; }
    }

}