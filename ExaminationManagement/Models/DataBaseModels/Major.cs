using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models.DataBaseModels
{
    public class Major
    {
        public int MajorId { get; set; }
        public string MajorName { get; set; }
        public double Credit { get; set; }
    }

    public class MajorComparer : EqualityComparer<Major>
    {
        public override bool Equals(Major x, Major y)
        {
            if (x.MajorName == y.MajorName)
                return true;
            return false;
        }

        public override int GetHashCode(Major obj)
        {
            return obj.MajorId.GetHashCode();
        }
    }
}