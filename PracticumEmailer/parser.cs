using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using LinqToExcel;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace PracticumEmailer
{
    class Parser
    {
        private string _file;
        private readonly Dictionary<String, Course> _requirements;
        private readonly DateTime _cutoff;
        private readonly IExcelQueryFactory _excelFactory;
        private readonly Dictionary<String, Student> _studentsInfo;
        private readonly Outlook.Application _myOutlook;
        private readonly bool? _isTest;
        

       public Parser(string file, DateTime cutoff, bool? test)
        {
            _file = file;
            try
            {
                _excelFactory = new ExcelQueryFactory(file);
            }
            catch (System.IO.IOException e)
            {
                throw e;
            }
            
           
            _requirements = new Dictionary<String, Course>();
            this._cutoff = cutoff;
            _studentsInfo = new Dictionary<String, Student>();
            _myOutlook = new Outlook.Application();
            _isTest = test;
            _requirements = CourseManager.readCourses();
        }

        public void StartParse()
        {
            string[] data = new string[9];

            foreach (var spreadSheetData in from d in _excelFactory.Worksheet()
                                            select new
                                            {
                                                CourseId = d["Course ID"],
                                                Name = d["Name"],
                                                TbExpiration = d["TB Test Exp"],
                                                FcsrExpiration = d["Liability Insurance Expiration"],
                                                FbiExpiration = d["FBI_Expiration"],
                                                Email = d["Email_Address"],
                                                MNumber = d["M-Number"],
                                                ProgramDescription = d["Program_Desc"]

                                            })
            {
                if (_studentsInfo.ContainsKey(spreadSheetData.MNumber))
                    _studentsInfo[spreadSheetData.MNumber].Courses.Add(spreadSheetData.CourseId);
                else
                    _studentsInfo.Add(spreadSheetData.MNumber, ProcessLine(data));

            }

            #region Debugging Code
 /* foreach (KeyValuePair<String, Student> entry in students_info)
                Console.WriteLine(entry.Value.ToString());

            Console.WriteLine(students_info.Count);

            foreach (var s in students_info.Values)
                s.prepEmail(false, pracRequire, stuRequire);

            foreach (var s in pracRequire.Keys)
                Console.WriteLine(s.ToString());*/
 #endregion

            foreach (var s in _studentsInfo.Values)
                s.PrepData(_requirements);

            int count = 0;
            
            foreach (var s in _studentsInfo.Values)
            {
                if (_isTest == true)
                {
                    s.DisplayMail(true);
                    if (count >= 5)
                        break;
                }
                else
                    s.DisplayMail(false);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(s.sendEmail), myOutlook);
                count += s.SendEmail(_myOutlook);
                
            }
            MessageBox.Show("Done Sending " + count + " emails!");
        }

       

        private Student ProcessLine(IList<string> line)
        {
            Student temp = new Student();
            temp.Courses.Add(line[0].Split(' ')[0]);
            temp.Name = line[1];
            temp.Email = line[6];
            temp.Major = line[8];
            //temp.mNum = line[7];

            temp.IsFbiCleared = cleared(line[5]);
            temp.IsFcsrCleared = cleared(line[4]);
            temp.IsLiabCleared = cleared(line[3]);
            temp.IsTbCleared = cleared(line[2]);

            return temp;
        }

        private bool cleared(string data)
        {
            if (string.IsNullOrEmpty(data))
                return false;

            if (Regex.IsMatch(data, "[0-9]*/[0-9]*/[0-9]*"))
            {
                try
                {
                    DateTime t = Convert.ToDateTime(data);

                    return t.CompareTo(_cutoff) >= 0;
                }
                catch (System.FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Invalid Date");
                }
                
            }

            return true;

        }
    }

}
