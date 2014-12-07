using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using PracticumEmailer.Domain;
using PracticumEmailer.Interfaces;

namespace PracticumEmailer.Ui.Managers
{
    [Export(typeof (IStudentManager))]
    public class StudentManager : IStudentManager
    {
        private readonly IDictionary<string, Student> _studentLookup;

        public StudentManager()
        {
            _studentLookup = new Dictionary<string, Student>();
        }
        public IEnumerable<Student> LoadAll(string file)
        {
            var studentData = DataTable.New.ReadLazy(new FileStream(file, FileMode.Open, FileAccess.Read)).Rows.Select(GetStudent);

            foreach (var student in studentData)
            {
                if (_studentLookup.ContainsKey(student.MNumber))
                {
                    if (student.Courses.Any())
                    {
                        _studentLookup[student.MNumber].Courses.Add(student.Courses.First());
                    }
                }
                else
                {
                    _studentLookup.Add(student.MNumber, student);
                }
            }

            return _studentLookup.Values;

        }

        public Requirements DetermineRequirements(IEnumerable<string> courses)
        {
            throw new NotImplementedException();
        }

        public bool IsCleared(Student student, Requirements requirements)
        {
            throw new NotImplementedException();
        }

        private Student GetStudent(Row row)
        {
            return new Student
            {
                Name = row["Name"],
                MNumber = row["M-Number"],
                Email = row["Email_Address"],
                Major = row["Program_Desc"],
                FbiExpiration = string.Join(",", row["FBI_DESE"], row["FBI_MOVECHS"], row["FBW_MOVECHS"]),
                LiabExpiration = row["Liability Insurance Expiration"],
                FcsrExpiration = row["FCSR Expiration Date"],
                TbExpiration = row["TB Test Exp"]
            };
        }
    }
}
