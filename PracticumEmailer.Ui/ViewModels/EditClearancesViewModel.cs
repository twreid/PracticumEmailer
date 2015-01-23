using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Newtonsoft.Json;
using PracticumEmailer.Domain;
using PracticumEmailer.Ui.Properties;

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

        public CourseViewModel CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                NotifyOfPropertyChange(() => CurrentItem);
            }
        }

        private void SaveCoursesToDisk(List<Course> courses)
        {
            using (var stream = new FileStream(_courseDataPath, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(courses, Formatting.Indented));
            }
        }
    }
}