using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using LinqToExcel;
using PracticumEmailer.Interfaces;
using Application = Microsoft.Office.Interop.Outlook.Application;
using Row = LinqToExcel.Row;

namespace PracticumEmailer
{
    internal class Parser
    {
        private readonly DateTime _cutoff;
        private readonly IEmailManager _emailManager;
        private readonly IExcelQueryFactory _excelFactory;
        private readonly bool? _isTest;
        private readonly Application _myOutlook;
        private readonly IStudentManager _studentManager;
        private readonly Task _studentTask;
        private readonly IDictionary<string, Domain.Student> _students;


        public Parser(string file, DateTime cutoff, bool? test, IEmailManager emailManager,
            IStudentManager studentManager)
        {
            try
            {
                _excelFactory = new ExcelQueryFactory(file);
            }
            catch (IOException e)
            {
                throw;
            }

            _emailManager = emailManager;
            _studentManager = studentManager;
            _cutoff = cutoff;
            _students = new Dictionary<string, Domain.Student>();
            _myOutlook = new Application();
            _isTest = test;

            _studentTask = Task.Factory.StartNew(PopulateStudents);
        }

        public void StartParse()
        {
            _studentTask.Wait();

            IEnumerable<MailMessage> emails = _students.Values.Select(s => _emailManager.GenerateEmail(s));

            if (_isTest.GetValueOrDefault(false))
            {
               ShowTestEmails(emails.Take(5)); 
            }

        }

        private void ShowTestEmails(IEnumerable<MailMessage> emails)
        {
            
        }

        private void PopulateStudents()
        {
            foreach (Row row in _excelFactory.Worksheet())
            {
                Domain.Student student = CreateStudentFromRow(row);

                if (_students.ContainsKey(student.MNumber))
                {
                    _students[student.MNumber].Courses.Add(student.Courses.First());
                }
            }
        }

        private Domain.Student CreateStudentFromRow(Row row)
        {
            var student = new Domain.Student
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

            student.Courses.Add(row["Course ID"].ToString().Split(' ')[0]);

            return student;
        }
    }
}