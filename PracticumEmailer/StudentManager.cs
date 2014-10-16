using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PracticumEmailer.Interfaces;

namespace PracticumEmailer
{
    public  class StudentManager : IStudentManager
    {
        public Requirements DetermineRequirements(Domain.Student student)
        {
            throw new NotImplementedException();
        }

        public bool IsCleared(Domain.Student student, Requirements requirements)
        {
            throw new NotImplementedException();
        }
    }
}
