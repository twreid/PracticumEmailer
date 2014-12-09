using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Domain
{
    public class Course
    {
        public string CourseId { get; set; }

        public bool FbiRequired { get; set; }

        public bool TbRequired { get; set; }

        public bool FcsrRequired { get; set; }

        public bool LiabRequired { get; set; }

        public bool IsPracticum { get; set; }
    }
}
