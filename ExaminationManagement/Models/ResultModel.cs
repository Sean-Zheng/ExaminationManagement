using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models
{
    public class ResultModel
    {
        public string name { get; set; }
        public List<double> data { get; set; }
        public ResultModel()
        {
            data = new List<double>();
        }
    }
}