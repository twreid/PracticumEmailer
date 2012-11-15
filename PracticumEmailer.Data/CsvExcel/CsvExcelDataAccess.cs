using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using LinqToExcel.Domain;
using PracticumEmailer.Business;
using PracticumEmailer.Data.Models;
using LinqToExcel;

namespace PracticumEmailer.Data.CsvExcel
{
    [Export(typeof(IStudentDataAccess))]
    public class CsvExcelDataAccess : IStudentDataAccess
    {
        private readonly IExcelQueryFactory _factory;
        private readonly IDictionary<string, Student> _students; 

        public CsvExcelDataAccess()
        {
            _students = new ConcurrentDictionary<string, Student>(Environment.ProcessorCount * 2, 201);

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

        [Import]
        public Mapper.Mapper Mapper { get; set; }

        #region Implementation of IDataAccess

        public IEnumerable<Student> GetCourseInfo(string file)
        {
            var courseInfos = _factory.Worksheet<StudentCourseInfo>().Select(s => s);

            foreach (Student student in courseInfos.Select(studentCourseInfo => Mapper.MapStudentFromDataLayer(studentCourseInfo)))
            {
                if(_students.ContainsKey(student.MNumber))
                {
                    foreach (string course in student.Courses)
                    {
                        _students[student.MNumber].Courses.Add(course);
                    }
                }
                else
                {
                    _students[student.MNumber] = student;
                }
            }

            return _students.Values.AsEnumerable();
        }

        #endregion
    }
}
