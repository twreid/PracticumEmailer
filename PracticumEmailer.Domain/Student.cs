using System;
using System.Collections.Generic;

namespace PracticumEmailer.Domain
{
    public class Student
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Major { get; set; }

        public string MNumber { get; set; }

        public string TbExpiration { get; set; }

        public string LiabExpiration { get; set; }

        public string FcsrExpiration { get; set; }

        public string FbiExpiration { get; set; }

        public ISet<String> Courses { get; set; } 

    }
}
