using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Animation;
using LinqToExcel;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace PracticumEmailer
{
    class Parser
    {
        private readonly Dictionary<String, Course> _requirements;
        private readonly DateTime _cutoff;
        private readonly IExcelQueryFactory _excelFactory;
        private readonly Dictionary<String, Student> _studentsInfo;
        private readonly Outlook.Application _myOutlook;
        private readonly bool? _isTest;
        

       public Parser(string file, DateTime cutoff, bool? test)
        {
            try
            {
                _excelFactory = new ExcelQueryFactory(file);
            }
            catch (System.IO.IOException e)
            {
                throw e;
            }
            
           
            _requirements = new Dictionary<String, Course>();
            _cutoff = cutoff;
            _studentsInfo = new Dictionary<String, Student>();
            _myOutlook = new Outlook.Application();
            _isTest = test;
            _requirements = CourseManager.readCourses();
        }

        public void StartParse()
        {

            foreach (var spreadSheetData in from d in _excelFactory.Worksheet()
                                            select new Student
                                                {
                                                    Name = d["Name"],
                                                    MNumber = d["M-Number"],
                                                    Email = d["Email_Address"],
                                                    CourseId = d["Course ID"].ToString().Split(' ')[0],
                                                    Major = d["Program_Desc"],
                                                    IsFbiCleared = IsFbiCleared(d["FBI_DESE"], d["FBI_MOVECHS"], d["FBW_MOVECHS"]),
                                                    IsLiabCleared = Cleared(d["Liability Insurance Expiration"]),
                                                    IsFcsrCleared = Cleared(d["FCSR Expiration Date"]),
                                                    IsTbCleared = Cleared(d["TB Test Exp"])
                                                })
            {
                if (_studentsInfo.ContainsKey(spreadSheetData.MNumber))
                {
                    _studentsInfo[spreadSheetData.MNumber].Courses.Add(spreadSheetData.CourseId);
                }
                else
                {
                    spreadSheetData.Courses.Add(spreadSheetData.CourseId);
                    _studentsInfo.Add(spreadSheetData.MNumber, spreadSheetData);
                }
                    

            }

            foreach (var s in _studentsInfo.Values)
            {
                s.PrepData(_requirements);
            }
                

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
                {
                    s.DisplayMail(false);
                }
                    
                
                count += s.SendEmail(_myOutlook);
                
            }

            MessageBox.Show("Done Sending " + count + " emails!");
        }

        private bool IsFbiCleared(params string[] fbiClearances)
        {
            return fbiClearances.Aggregate(false, (current, fbiClearance) => current || Cleared(fbiClearance));
        }

        private bool Cleared(string data)
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
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Invalid Date");
                }
                
            }

            return true;

        }
    }

}
