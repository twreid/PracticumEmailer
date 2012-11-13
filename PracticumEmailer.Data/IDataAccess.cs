using System.Collections.Generic;
using PracticumEmailer.Data.Models;

namespace PracticumEmailer.Data
{
    public interface IDataAccess
    {
        IEnumerable<StudentCourseInfo> GetCourseInfo(string file);
    }
}
