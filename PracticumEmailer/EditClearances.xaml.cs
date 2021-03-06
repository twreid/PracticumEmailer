﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PracticumEmailer
{
    /// <summary>
    ///     Interaction logic for EditClearances.xaml
    /// </summary>
    public partial class EditClearances : Window
    {
        private readonly BindingList<Course> _courses;

        public EditClearances()
        {
            InitializeComponent();

            List<Course> courses = CourseManager.GetCourseList().OrderBy(c => c.CourseId).ToList();

            _courses = new BindingList<Course>(courses)
            {
                AllowEdit = true,
                AllowNew = true,
                AllowRemove = true,
            };

            CourseGrid.DataContext = _courses;
        }

        private void EditClearances_Closing(object sender, CancelEventArgs e)
        {
            CourseManager.SaveCourses(_courses.ToList());
        }
    }
}