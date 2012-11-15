using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PracticumEmailer.Business;
using PracticumEmailer.Data.Models;
using AutoMapper;

namespace PracticumEmailer.Data.Mapper
{
    public class Mapper
    {
        public Mapper()
        {
            AutoMapper.Mapper.CreateMap<StudentCourseInfo, Student>();
            AutoMapper.Mapper.CreateMap<StudentCourseInfo, Clearances>();
        }
        
        public Student MapStudentFromDataLayer(StudentCourseInfo info)
        {

            Student student = AutoMapper.Mapper.Map<StudentCourseInfo, Student>(info);
            student.Clearances = MapToClearances(info);
            student.Courses.Add(info.CourseId);
            return student;

        }

        private Clearances MapToClearances(StudentCourseInfo info)
        {
            return AutoMapper.Mapper.Map<StudentCourseInfo, Clearances>(info);
        }

    }
}
