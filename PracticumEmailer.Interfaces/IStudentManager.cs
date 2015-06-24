using PracticumEmailer.Domain;
using System;
using System.Collections.Generic;

namespace PracticumEmailer.Interfaces
{
    public interface IStudentManager
    {
        IEnumerable<Student> LoadAll(string file);

        Requirements DetermineRequirements(IEnumerable<string> courses);

        bool IsCleared(Student student, Requirements requirements);

        Requirements DetermineEmails(Student student, Requirements requirements, DateTime cutOff);
    }
}