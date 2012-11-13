using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PracticumEmailer.Business
{
    public class Student : BusinessObject
    {
        public Student()
        {
            Courses = new List<string>();
        }

        public string Name { get; set; }
        public string MNumber { get; set; }
        public string ProgramDescription { get; set; }
        public string Email { get; set; }
        public Clearances Clearances { get; set; }
        public IList<string> Courses { get; private set; } 
    }
}
