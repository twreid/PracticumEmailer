using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Office.Interop.Outlook;


namespace PracticumEmailer
{
    public class Student
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Major { get; set; }
        //public string mNum;
        public List<String> Courses { get; set; }
        public bool IsTbCleared { get; set; }
        public bool IsLiabCleared { get; set; }
        public bool IsFcsrCleared { get; set; }
        public bool IsFbiCleared { get; set; }
        private readonly string _curdir;

        private bool _needFbi, _needTb, _needLiab, _needFcsr, _isPracticum, _isDisp;

        public Student()
        {
            Courses = new List<String>();
            _needFbi = false;
            _needTb = false;
            _needLiab = false;
            _needFcsr = false;
            _isPracticum = false;
            _isDisp = true;
            _curdir = Assembly.GetExecutingAssembly().Location;
            _curdir = _curdir.Remove(_curdir.LastIndexOf('\\'));
        }

        public override String ToString()
        {
            return "Name: " + Name + " Email: " + Email + " Courses: " + Courses.ToString();
        }

        public void PrepData(Dictionary<String, Course> req)
        {
            SetNeeds(req);
        }

        private void SetNeeds(IDictionary<string, Course> req)
        {
            foreach (string courseId in Courses.Where(courseId => !courseId.Contains("KIN") || !Major.Contains("Exercise & Mov")).Where(req.ContainsKey))
            {
                _isPracticum = req[courseId].IsPracticum;


                _needFbi = req[courseId].FBI;
                _needFcsr = req[courseId].FCSR;
                _needLiab = req[courseId].LIAB;
                _needTb = req[courseId].TB;
            }
        }


        public void DisplayMail(bool disp)
        {
            _isDisp = disp;
        }

        public int SendEmail(Object outlook)
        {
            System.IO.StreamWriter badEmails = new System.IO.StreamWriter(_curdir + "/bad emails.txt", true);
            string header = System.IO.File.ReadAllText(_curdir + "/header.html");
            string strCourses = "";
            bool needsEmail = false;
            int ret = 0;
            int countOfRequired = 0;

            foreach (string course in Courses)
            {
                strCourses += course;
                strCourses += ", ";
            }
            strCourses.Remove(strCourses.LastIndexOf(","));
            header = header.Replace("%student_name%", this.Name);
            header = header.Replace("%courses%", strCourses);

            header = header.Replace("%plural_courses%", Courses.Count > 1 ? "courses" : "course");


            header = header.Replace("%class_type%", _isPracticum ? "a practicum" : "student teaching");


            Application myOut = (Application)outlook;

            MailItem msg = (MailItem)myOut.CreateItem(OlItemType.olMailItem);
            msg.Subject = "Required Clearance Documents";
            msg.Importance = OlImportance.olImportanceHigh;
            Recipient to = msg.Recipients.Add(this.Email);
            if (!to.Resolve())
            {
                badEmails.WriteLine(this.Name + "\t" + this.Email + "\t");
            }

            if (_needFcsr && !IsFcsrCleared)
            {
                countOfRequired++;
            }


            if (_needTb && !IsTbCleared)
            {
                countOfRequired++;
            }


            if (_needLiab && !IsLiabCleared)
            {
                countOfRequired++;
            }


            if (_needFbi && !IsFbiCleared)
            {
                countOfRequired++;
            }


            header = header.Replace("%plural_documents%", countOfRequired > 1 ? "documents" : "document");

            msg.HTMLBody += header;
            if (_needFcsr && !IsFcsrCleared)
            {
                msg.HTMLBody += System.IO.File.ReadAllText(_curdir + "/fcsr.html");
                needsEmail = true;
                countOfRequired++;
            }

            if (_needTb && !IsTbCleared)
            {
                msg.HTMLBody += System.IO.File.ReadAllText(_curdir + "/tb.html");
                needsEmail = true;
                countOfRequired++;
            }

            if (_needLiab && !IsLiabCleared)
            {
                msg.HTMLBody += System.IO.File.ReadAllText(_curdir + "/pli.html");
                needsEmail = true;
                countOfRequired++;
            }

            if (_needFbi && !IsFbiCleared)
            {
                msg.HTMLBody += System.IO.File.ReadAllText(_curdir + "/fbi.html");
                needsEmail = true;
                countOfRequired++;
            }


            msg.HTMLBody += System.IO.File.ReadAllText(_curdir + "/footer.html");

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
    }
}