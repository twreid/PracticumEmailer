using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PracticumEmailer.Data.Models
{
    public class Course
    {
        public string CourseId { get; set; }
        public bool Fbi { get; set; }
        public bool Tb { get; set; }
        public bool Fcsr { get; set; }
        public bool Pli { get; set; }
        public CourseType CourseType { get; set; }

    }
}
