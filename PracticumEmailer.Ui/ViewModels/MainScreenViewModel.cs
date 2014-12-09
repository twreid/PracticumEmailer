using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Caliburn.Micro;
using Caliburn.Micro.Extras;
using PracticumEmailer.Domain;
using PracticumEmailer.Interfaces;

namespace PracticumEmailer.Ui.ViewModels
{
    public class MainScreenViewModel : Screen
    {
        private readonly IStudentManager _studentManager;
        private DateTime _cutOff;
        private string _dataFile;
        private string _displayName;

        [ImportingConstructor]
        public MainScreenViewModel(IStudentManager studentManager)
        {
            _displayName = "Practicum Emailer";
            _cutOff = DateTime.Now;
            _studentManager = studentManager;
        }

        public override string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        public string DataFile
        {
            get { return _dataFile; }
            set
            {
                _dataFile = value;
                NotifyOfPropertyChange(() => DataFile);
            }
        }

        public DateTime CutOff
        {
            get { return _cutOff; }
            set
            {
                _cutOff = value;
                NotifyOfPropertyChange(() => CutOff);
            }
        }

        public OpenFileResult<FileInfo> OpenFile()
        {
            OpenFileResult<FileInfo> openFileResult =
                OpenFileResult.OneFile("Please Choose the data File.").FilterFiles("CSV Files (*.csv)|*.csv");
            openFileResult.Completed += (sender, args) =>
            {
                var openFile = sender as OpenFileResult<FileInfo>;
                if (openFile != null) DataFile = openFile.Result.FullName;
            };

            return openFileResult;
        }

        public void AssignCutOff(SelectedDatesCollection dates)
        {
            _cutOff = dates.First();
            NotifyOfPropertyChange(() => CutOff);
        }

        public void Start(string dataFile)
        {
            IEnumerable<Student> students = _studentManager.LoadAll(_dataFile);

            foreach (Student student in students)
            {
                Requirements studentRequirements = _studentManager.DetermineRequirements(student.Courses);

                Requirements emailRequirements = _studentManager.DetermineEmails(student, studentRequirements, _cutOff);
            }
        }

        public bool CanStart(string dataFile)
        {
            return File.Exists(dataFile);
        }
    }
}