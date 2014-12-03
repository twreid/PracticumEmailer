using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace PracticumEmailer
{
    public class CourseManager
    {
        private const string FileName = "courses.xml";

        public static void SaveCourses(List<Course> l)
        {
            var s = new XmlSerializer(typeof (List<Course>), "Courses");
            TextWriter t = new StreamWriter(FileName);

            s.Serialize(t, l);

            t.Close();
        }

        public static Dictionary<string, Course> ReadCourses()
        {
            var s = new XmlSerializer(typeof (List<Course>), "Courses");

            using (TextReader r = new StreamReader(FileName))
            {
                var l = (List<Course>) s.Deserialize(r);

                return l.Where(c => !string.IsNullOrEmpty(c.CourseId)).ToDictionary(c => c.CourseId);
            }
        }

        public static List<Course> GetCourseList()
        {
            var s = new XmlSerializer(typeof (List<Course>), "Courses");

            using (TextReader r = new StreamReader(FileName))
            {
                return (List<Course>) s.Deserialize(r);
            }
        }
    }
}