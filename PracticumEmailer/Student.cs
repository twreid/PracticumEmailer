using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Office.Interop.Outlook;

namespace PracticumEmailer
{
    public class Student
    {
        private readonly string _curdir;
        private bool _isDisp;
        private bool _isPracticum;
        private bool _needFbi;
        private bool _needFcsr;
        private bool _needLiab;
        private bool _needTb;

        public Student()
        {
            Courses = new HashSet<String>();
            _needFbi = false;
            _needTb = false;
            _needLiab = false;
            _needFcsr = false;
            _isPracticum = false;
            _isDisp = true;
            _curdir = Assembly.GetExecutingAssembly().Location;
            _curdir = _curdir.Remove(_curdir.LastIndexOf('\\'));
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Major { get; set; }
        public ISet<String> Courses { get; private set; }
        public bool IsTbCleared { get; set; }
        public bool IsLiabCleared { get; set; }
        public bool IsFcsrCleared { get; set; }
        public bool IsFbiCleared { get; set; }
        public string CourseId { get; set; }
        public string MNumber { get; set; }

        public override String ToString()
        {
            return "Name: " + Name + " Email: " + Email + " Courses: " + Courses;
        }

        public void PrepData(Dictionary<String, Course> req)
        {
            SetNeeds(req);
        }

        private void SetNeeds(IDictionary<string, Course> req)
        {
            foreach (
                string courseId in
                    Courses.Where(courseId => !courseId.Contains("KIN") || !Major.Contains("Exercise & Mov"))
                        .Where(req.ContainsKey))
            {
                _isPracticum = req[courseId].IsPracticum;


                _needFbi = req[courseId].Fbi;
                _needFcsr = req[courseId].Fcsr;
                _needLiab = req[courseId].Liab;
                _needTb = req[courseId].Tb;
            }
        }


        public void DisplayMail(bool disp)
        {
            _isDisp = disp;
        }

        public int SendEmail(Object outlook)
        {
            var badEmails = new StreamWriter(_curdir + "/bad emails.txt", true);
            string header = File.ReadAllText(_curdir + "/header.html");
            string strCourses = string.Concat(Courses.Select(s => s + ","));
            bool needsEmail = false;
            int ret = 0;

            strCourses = strCourses.TrimEnd(',');
            header = header.Replace("%student_name%", Name);
            header = header.Replace("%courses%", strCourses);

            header = header.Replace("%plural_courses%", Courses.Count > 1 ? "courses" : "course");


            header = header.Replace("%class_type%", _isPracticum ? "a practicum" : "student teaching");


            var myOut = (Application) outlook;

            var msg = (MailItem) myOut.CreateItem(OlItemType.olMailItem);
            msg.Subject = "Required Clearance Documents";
            msg.Importance = OlImportance.olImportanceHigh;
            Recipient to = msg.Recipients.Add(Email);

            if (!to.Resolve())
            {
                badEmails.WriteLine(Name + "\t" + Email + "\t");
            }


            header = header.Replace("%plural_documents%", CountOfRequired() > 1 ? "documents" : "document");

            msg.HTMLBody += header;
            if (_needFcsr && !IsFcsrCleared)
            {
                msg.HTMLBody += File.ReadAllText(_curdir + "/fcsr.html");
                needsEmail = true;
            }

            if (_needTb && !IsTbCleared)
            {
                msg.HTMLBody += File.ReadAllText(_curdir + "/tb.html");
                needsEmail = true;
            }

            if (_needLiab && !IsLiabCleared)
            {
                msg.HTMLBody += File.ReadAllText(_curdir + "/pli.html");
                needsEmail = true;
            }

            if (_needFbi && !IsFbiCleared)
            {
                msg.HTMLBody += File.ReadAllText(_curdir + "/fbi.html");
                needsEmail = true;
            }


            msg.HTMLBody += File.ReadAllText(_curdir + "/footer.html");

            if (needsEmail)
            {
                if (_isDisp)
                {
                    msg.Display();
                }
                else
                {
                    msg.Send();
                }

                ret = 1;
            }

            badEmails.Close();

            return ret;
        }

        private int CountOfRequired()
        {
            int count = 0;
            if (_needFcsr && !IsFcsrCleared)
            {
                count++;
            }


            if (_needTb && !IsTbCleared)
            {
                count++;
            }


            if (_needLiab && !IsLiabCleared)
            {
                count++;
            }


            if (_needFbi && !IsFbiCleared)
            {
                count++;
            }

            return count;
        }
    }
}