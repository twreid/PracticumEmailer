using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PracticumEmailer.Data.Models;

namespace PracticumEmailer.Data
{
    public interface IDataAccess
    {
        IEnumerable<StudentCourseInfo> GetCourseInfo(string file);
    }
}
