using System;
using Caliburn.Micro;
using PracticumEmailer.Domain;

namespace PracticumEmailer.Ui.ViewModels
{
    public class CourseViewModel : PropertyChangedBase
    {
        private readonly Course _course;

        public CourseViewModel(Course course)
        {
            _course = course;
        }

        public String CourseId
        {
            get { return _course.CourseId; }
            set
            {
                _course.CourseId = value;
                NotifyOfPropertyChange(() => CourseId);
            }
        }

        public bool IsPracticum
        {
            get { return _course.IsPracticum; }
            set
            {
                _course.IsPracticum = value;
                NotifyOfPropertyChange(() => IsPracticum);
            }
        }

        public bool FbiRequired
        {
            get { return _course.FbiRequired; }
            set
            {
                _course.FbiRequired = value;
                NotifyOfPropertyChange(() => FbiRequired);
            }
        }

        public bool FcsrRequired
        {
            get { return _course.FcsrRequired; }
            set
            {
                _course.FcsrRequired = value;
                NotifyOfPropertyChange(() => FcsrRequired);
            }
        }

        public bool LiabRequired
        {
            get { return _course.LiabRequired; }
            set
            {
                _course.LiabRequired = value;
                NotifyOfPropertyChange(() => LiabRequired);
            }
        }

        public bool TbRequired
        {
            get { return _course.TbRequired; }
            set
            {
                _course.TbRequired = value;
                NotifyOfPropertyChange(() => TbRequired);
            }
        }

        public Course Course
        {
            get { return _course; }
        }
    }
}