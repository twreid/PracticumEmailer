using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticumEmailer.Domain;

namespace PracticumEmailer.Interfaces
{
    public interface IStudentManager
    {

        IEnumerable<Student> LoadAll(string file);
        
        Requirements DetermineRequirements(IEnumerable<string> courses);

        bool IsCleared(Student student, Requirements requirements);

        Requirements DetermineEmails(Student student, Requirements requirements, DateTime cutOff);

    }
}
