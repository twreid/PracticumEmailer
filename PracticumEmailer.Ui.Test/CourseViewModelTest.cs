using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PracticumEmailer.Domain;
using PracticumEmailer.Ui.ViewModels;
using PracticumEmailer.Ui.Views;
using Xunit;

namespace PracticumEmailer.Ui.Test
{
    public class CourseViewModelTest
    {
        [Fact]
        public void CourseIdRaisesPropertyChanged()
        {
            var viewModel = new CourseViewModel(new Course());
            string property = null;
            const string expected = "1234";

            viewModel.PropertyChanged += (sender, args) =>
            {
                property = args.PropertyName;
            };

            viewModel.CourseId = expected;

            Assert.Equal("CourseId", property);
            Assert.Equal(expected, viewModel.CourseId);
        }

        [Fact]
        public void IsPracticumRaisesPropertyChanged()
        {
            var viewModel = new CourseViewModel(new Course());
            string property = null;
            const bool expected = true;

            viewModel.PropertyChanged += (sender, args) =>
            {
                property = args.PropertyName;
            };

            viewModel.IsPracticum = expected;

            Assert.Equal("IsPracticum", property);
            Assert.True(viewModel.IsPracticum);
        }
    }
}
