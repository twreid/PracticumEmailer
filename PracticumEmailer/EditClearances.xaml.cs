using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace PracticumEmailer
{
    /// <summary>
    /// Interaction logic for EditClearances.xaml
    /// </summary>
    public partial class EditClearances : Window
    {
        private BindingList<Course> courses;

        

        public EditClearances()
        {
            InitializeComponent();
            List<Course> c = CourseManager.getCourseList();
            c.Sort(SortCourses);
            courses = new BindingList<Course>(c);
            

            dataGrid1.DataContext = courses;           
            
        }

        void EditClearances_Closing(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("Window Closing");

            CourseManager.saveCourses(new List<Course>(courses));
        }

        public int SortCourses(Course c1, Course c2)
        {
            return c1.CourseId.CompareTo(c2.CourseId);
        }

       }
}
