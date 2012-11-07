using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace PracticumEmailer
{
    static class CourseManager
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
                Dictionary<string, Course> d = new Dictionary<string, Course>();

                foreach (Course c in l)
                {
                    if(!string.IsNullOrEmpty(c.CourseId)
                        d.Add(c.CourseId, c);
                }

                return d;
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
