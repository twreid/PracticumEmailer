using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using LinqToExcel.Domain;
using PracticumEmailer.Data.Models;
using LinqToExcel;

namespace PracticumEmailer.Data.CsvExcel
{
    [Export(typeof(IDataAccess))]
    public class CsvExcelDataAccess : IDataAccess
    {
        private readonly IExcelQueryFactory _factory;

        public CsvExcelDataAccess()
        {
            _factory = new ExcelQueryFactory {DatabaseEngine = DatabaseEngine.Ace, StrictMapping = false};

            _factory.AddMapping<StudentCourseInfo>(info => info.CourseId, ColumnMappings.Default.CourseId);
            _factory.AddMapping<StudentCourseInfo>(info => info.Email, ColumnMappings.Default.Email);
            _factory.AddMapping<StudentCourseInfo>(info => info.Name, ColumnMappings.Default.Name);
            _factory.AddMapping<StudentCourseInfo>(info => info.MNumber, ColumnMappings.Default.MNumber);
            _factory.AddMapping<StudentCourseInfo>(info => info.ProgramDescription, ColumnMappings.Default.ProgramDescription);
            _factory.AddMapping<StudentCourseInfo>(info => info.TbExpiration, ColumnMappings.Default.TbExpiration);
            _factory.AddMapping<StudentCourseInfo>(info => info.FbiExpiration, ColumnMappings.Default.FbiExpiration);
            _factory.AddMapping<StudentCourseInfo>(info => info.FcsrExpiration, ColumnMappings.Default.FcsrExpiration);
            _factory.AddMapping<StudentCourseInfo>(info => info.PliExpiration, ColumnMappings.Default.PliExpiration);
         }

        #region Implementation of IDataAccess

        public IEnumerable<StudentCourseInfo> GetCourseInfo(string file)
        {
            return _factory.Worksheet<StudentCourseInfo>().Select(s => s);
        }

        #endregion
    }
}
