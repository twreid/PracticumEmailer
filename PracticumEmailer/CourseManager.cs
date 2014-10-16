using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace PracticumEmailer
{
    public class CourseManager
    {
        //Takes a dictionary of courses and the object course and writes it to xml
        public static void saveCourses(Dictionary<string, Course> d)
        {
            List<Course> l = d.Select(kvp => kvp.Value).ToList();

            XmlSerializer s = new XmlSerializer(typeof(List<Course>), "Courses");
            TextWriter t = new StreamWriter("courses.xml");

            s.Serialize(t, l);

            t.Close();
            
        }

        public static void saveCourses(List<Course> l)
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Course>), "Courses");
            TextWriter t = new StreamWriter("courses.xml");

            s.Serialize(t, l);

            t.Close();

        }

        //Returns a dictionary of the courses and their objects from the xml file.
        public static Dictionary<string, Course> readCourses()
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Course>), "Courses");

            using (TextReader r = new StreamReader("courses.xml"))
            {
                List<Course> l = (List<Course>)s.Deserialize(r);

                return l.Where(c => !string.IsNullOrEmpty(c.CourseId)).ToDictionary(c => c.CourseId);
            }
        }

        public static List<Course> getCourseList()
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Course>), "Courses");

            using (TextReader r = new StreamReader("courses.xml"))
            {
                return (List<Course>)s.Deserialize(r);             

            }
        }
    }
}
