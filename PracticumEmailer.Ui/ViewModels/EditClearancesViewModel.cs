using Caliburn.Micro;
using Newtonsoft.Json;
using PracticumEmailer.Domain;
using PracticumEmailer.Ui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

namespace PracticumEmailer.Ui.ViewModels
{
    public class EditClearancesViewModel : Screen
    {
        private readonly string _courseDataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.CourseDataFile);

        private CourseViewModel _currentItem;

        [ImportingConstructor]
        public EditClearancesViewModel()
        {
            Courses =
                new BindableCollection<CourseViewModel>(
                    JsonConvert.DeserializeObject<List<Course>>(File.ReadAllText(_courseDataPath, Encoding.UTF8))
                        .Select(c => new CourseViewModel(c)));
        }

        public BindableCollection<CourseViewModel> Courses { get; set; }

        public CourseViewModel CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                _currentItem = value;
                NotifyOfPropertyChange(() => CurrentItem);
            }
        }

        public void SaveCourses()
        {
            List<Course> courses =
                Courses.Select(cvm => cvm.Course).Where(c => !string.IsNullOrEmpty(c.CourseId)).ToList();

            SaveCoursesToDisk(courses);
        }

        public void AddCourse()
        {
            Courses.Add(new CourseViewModel(new Course()));
            NotifyOfPropertyChange(() => Courses);
            CurrentItem = Courses.Last();
        }

        private void SaveCoursesToDisk(List<Course> courses)
        {
            File.WriteAllText(_courseDataPath, JsonConvert.SerializeObject(courses.ToArray(), Formatting.Indented));
        }
    }
}