using DataAccess;
using Newtonsoft.Json;
using PracticumEmailer.Domain;
using PracticumEmailer.Interfaces;
using PracticumEmailer.Ui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PracticumEmailer.Ui.Managers
{
    [Export(typeof (IStudentManager))]
    public class StudentManager : IStudentManager
    {
        private readonly string _courseDataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.CourseDataFile);

        private readonly IDictionary<string, Course> _courseLookup;
        private readonly IDictionary<string, Student> _studentLookup;

        public StudentManager()
        {
            _studentLookup = new Dictionary<string, Student>();
            _courseLookup =
                JsonConvert.DeserializeObject<List<Course>>(File.ReadAllText(_courseDataPath, Encoding.UTF8))
                    .ToDictionary(cl => cl.CourseId);
        }

        public IEnumerable<Student> LoadAll(string file)
        {
            IEnumerable<Student> studentData =
                DataTable.New.ReadLazy(new FileStream(file, FileMode.Open, FileAccess.Read)).Rows.Select(GetStudent);

            foreach (Student student in studentData.Where(s => !s.Major.Contains("Excercise & Mov")))
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
            var studentRequirements = Requirements.None;

            foreach (Course c in courses.Where(c => !c.Contains("KIN")).Select(course => _courseLookup[course]))
            {
                if (c.IsPracticum)
                {
                    studentRequirements |= Requirements.Practicum;
                }

                if (c.FbiRequired)
                {
                    studentRequirements |= Requirements.Fbi;
                }

                if (c.FcsrRequired)
                {
                    studentRequirements |= Requirements.Fcsr;
                }

                if (c.LiabRequired)
                {
                    studentRequirements |= Requirements.Liab;
                }

                if (c.TbRequired)
                {
                    studentRequirements |= Requirements.Tb;
                }
            }

            return studentRequirements;
        }

        public bool IsCleared(Student student, Requirements requirements)
        {
            throw new NotImplementedException();
        }

        public Requirements DetermineEmails(Student student, Requirements requirements, DateTime cutOff)
        {
            var emailsNeeded = Requirements.None;

            if (requirements.HasFlag(Requirements.Fbi))
            {
                if (
                    !student.FbiExpiration.Split(',')
                        .Aggregate(false, (current, date) => current || IsCleared(date, cutOff)))
                {
                    emailsNeeded |= Requirements.Fbi;
                }
            }

            if (requirements.HasFlag(Requirements.Fcsr))
            {
                if (string.IsNullOrEmpty(student.FcsrExpiration) || string.IsNullOrEmpty(student.FcsrExpiration.Trim()))
                {
                    emailsNeeded |= Requirements.Fcsr;
                }
            }

            if (requirements.HasFlag(Requirements.Liab))
            {
                if (!IsCleared(student.LiabExpiration, cutOff))
                {
                    emailsNeeded |= Requirements.Liab;
                }
            }

            if (requirements.HasFlag(Requirements.Tb))
            {
                if (!IsCleared(student.TbExpiration, cutOff))
                {
                    emailsNeeded |= Requirements.Tb;
                }
            }

            return emailsNeeded;
        }

        private Student GetStudent(Row row)
        {
            var student = new Student
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

            student.Courses.Add(row["Course ID"].Split(' ')[0]);

            return student;
        }

        private bool IsCleared(string expiration, DateTime cutOff)
        {
            if (string.IsNullOrEmpty(expiration))
            {
                return false;
            }

            if (Regex.IsMatch(expiration, "[0-9]*/[0-9]*/[0-9]*"))
            {
                try
                {
                    DateTime date = Convert.ToDateTime(expiration);

                    return date.CompareTo(cutOff) >= 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}