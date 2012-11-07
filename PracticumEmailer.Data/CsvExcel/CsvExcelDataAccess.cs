using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToExcel.Domain;
using PracticumEmailer.Data.Models;
using LinqToExcel;

namespace PracticumEmailer.Data.CsvExcel
{
    public class CsvExcelDataAccess : IDataAccess
    {
        private readonly IExcelQueryFactory _factory;

        public CsvExcelDataAccess()
        {
            _factory = new ExcelQueryFactory {DatabaseEngine = DatabaseEngine.Ace, StrictMapping = false};

            _factory.AddMapping<StudentCourseInfo>(info => info.CourseId, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.Email, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.Name, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.MNumber, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.ProgramDescription, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.TbExpiration, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.FbiExpiration, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.FcsrExpiration, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.PliExpiration, "");
            _factory.AddMapping<StudentCourseInfo>(info => info.HighwayPatrolCheck, "");
        }

        #region Implementation of IDataAccess

        public IEnumerable<StudentCourseInfo> GetCourseInfo(string file)
        {
            return Enumerable.Empty<StudentCourseInfo>();
        }

        #endregion
    }
}
