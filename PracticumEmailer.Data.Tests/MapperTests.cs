using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticumEmailer.Business;
using PracticumEmailer.Data.Mapper;
using PracticumEmailer.Data.Models;

namespace PracticumEmailer.Data.Tests
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void MapStudentFromDataLayerMapsCorrectly()
        {
            Mapper.Mapper map = new Mapper.Mapper();

            StudentCourseInfo courseInfo = new StudentCourseInfo
                {
                    CourseId = "ENG111",
                    Email = "fake@gmail.com",
                    FbiExpiration = "10-10-2012",
                    FcsrExpiration = "10-10-2012",
                    MNumber = "M00000000",
                    Name = "Fake, John F",
                    PliExpiration = "10-10-2012",
                    ProgramDescription = "Computer Science",
                    TbExpiration = "10-10-2012"
                };

            Student student = map.MapStudentFromDataLayer(courseInfo);

            Assert.AreEqual(student.Email, courseInfo.Email);
            Assert.AreEqual(student.MNumber, courseInfo.MNumber);
            Assert.AreEqual(student.Name, courseInfo.Name);
            Assert.AreEqual(student.ProgramDescription, courseInfo.ProgramDescription);
            Assert.AreEqual(student.Clearances.FbiExpiration, courseInfo.FbiExpiration);
            Assert.AreEqual(student.Clearances.FcsrExpiration, courseInfo.FcsrExpiration);
            Assert.AreEqual(student.Clearances.PliExpiration, courseInfo.PliExpiration);
            Assert.AreEqual(student.Clearances.TbExpiration, courseInfo.TbExpiration);
            Assert.IsTrue(student.Courses.Contains(courseInfo.CourseId));
        }
        
    }
}
