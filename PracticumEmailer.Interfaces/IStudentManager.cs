using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticumEmailer.Interfaces
{
    [Flags]
    public enum Requirements
    {
        None,
        Fbi,
        Fcsr,
        Liab,
        Tb,
    }

    public interface IStudentManager
    {

        IEnumerable<Domain.Student> LoadAll(string file);
        
        Requirements DetermineRequirements(IEnumerable<string> courses);

        bool IsCleared(Domain.Student student, Requirements requirements);

    }
}
