using PracticumEmailer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PracticumEmailer
{
    public  class StudentManager : IStudentManager
    {
        private readonly CourseManager _courseManager;
        private readonly IDictionary<string, Course> _courses;
        private readonly DateTime _dateLimit;

        public StudentManager(CourseManager courseManager, DateTime dateLimit)
        {
            _courseManager = courseManager;
            _courses = CourseManager.ReadCourses();
            _dateLimit = dateLimit;
        }

        public Requirements DetermineRequirements(IEnumerable<string> courses)
        {
            var currentRequirements = Requirements.None;

            foreach (var courseRequirements in courses.Select(course => _courses[course]))
            {
                if (courseRequirements.Fbi)
                {
                    currentRequirements |= Requirements.Fbi;
                }

                if (courseRequirements.Fcsr)
                {
                    currentRequirements |= Requirements.Fcsr;
                }

                if (courseRequirements.Liab)
                {
                    currentRequirements |= Requirements.Liab;
                }

                if (courseRequirements.Tb)
                {
                    currentRequirements |= Requirements.Tb;
                }
            }

            return currentRequirements;
        }

        public bool IsCleared(Domain.Student student, Requirements requirements)
        {
            if (requirements.HasFlag(Requirements.Fbi))
            {
                return IsFbiCleared(student.FbiExpiration);
            }

            if (requirements.HasFlag(Requirements.Fcsr))
            {
                return IsFcsrCleared(student.FcsrExpiration);
            }
            
            if (requirements.HasFlag(Requirements.Liab))
            {
                return IsLiabCleared(student.LiabExpiration);
            }

            if (requirements.HasFlag(Requirements.Tb))
            {
                return IsTbCleared(student.TbExpiration);
            }

            return true;

        }

        private bool IsTbCleared(string tbExpiration)
        {
            return IsCleared(tbExpiration);
        }

        private bool IsLiabCleared(string liabExpiration)
        {
            return IsCleared(liabExpiration);
        }

        private bool IsFcsrCleared(string fcsrExpiration)
        {
            return IsCleared(fcsrExpiration);
        }

        private bool IsFbiCleared(string fbiExpiration)
        {
            return fbiExpiration.Split(',').Aggregate(false, (current, date) => current || IsCleared(date));
        }

        private bool IsCleared(string expiration)
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

                    return date.CompareTo(_dateLimit) >= 0;
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
