using System.Collections.Generic;
using PracticumEmailer.Business;

namespace PracticumEmailer.Data
{
    public interface IStudentDataAccess
    {
        IEnumerable<Student> GetCourseInfo(string file);
    }
}
