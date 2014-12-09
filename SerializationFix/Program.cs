using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PracticumEmailer;
using C = PracticumEmailer.Domain.Course;


namespace SerializationFix
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlSerializer = new XmlSerializer(typeof (List<PracticumEmailer.Course>), "Courses");

            var courses = xmlSerializer.Deserialize(new FileStream("D:\\courses.xml", FileMode.Open, FileAccess.Read)) as List<Course>;

            var jsonCourses = courses.Select(course => new C
            {
                CourseId = course.CourseId, FbiRequired = course.Fbi, FcsrRequired = course.Fcsr, IsPracticum = course.IsPracticum, LiabRequired = course.Liab, TbRequired = course.Tb
            }).ToList();

            using (var fs = new FileStream("D:\\courses.json", FileMode.CreateNew, FileAccess.Write))
            using (var writer = new StreamWriter(fs))
            {
                writer.Write(JsonConvert.SerializeObject(jsonCourses, Formatting.Indented));
            }
        }
    }
}
