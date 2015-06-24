using Caliburn.Micro;
using Caliburn.Micro.Extras;
using PracticumEmailer.Domain;
using PracticumEmailer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Controls;

namespace PracticumEmailer.Ui.ViewModels
{
    public class MainScreenViewModel : Screen
    {
        private readonly IEnumerable<Lazy<IEmailManager, IEmailManagerCapabilities>> _emailManagers;
        private readonly IStudentManager _studentManager;
        private DateTime _cutOff;
        private string _dataFile;
        private string _displayName;
        private EmailHandler _selectedEmailHandler;

        [ImportingConstructor]
        public MainScreenViewModel(IStudentManager studentManager,
            [ImportMany] IEnumerable<Lazy<IEmailManager, IEmailManagerCapabilities>> emailManagers)
        {
            _displayName = "Practicum Emailer";
            _cutOff = DateTime.Now;
            _studentManager = studentManager;
            _emailManagers = emailManagers;
            _selectedEmailHandler = _emailManagers.First().Metadata.Handler;
        }

        public override string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        public string DataFile
        {
            get
            {
                return _dataFile;
            }
            set
            {
                _dataFile = value;
                NotifyOfPropertyChange(() => DataFile);
            }
        }

        public DateTime CutOff
        {
            get
            {
                return _cutOff;
            }
            set
            {
                _cutOff = value;
                NotifyOfPropertyChange(() => CutOff);
            }
        }

        public IEnumerable<EmailHandler> EmailHandlers
        {
            get { return _emailManagers.Select(e => e.Metadata.Handler); }
        }

        public EmailHandler SelectedEmailHandler
        {
            get
            {
                return _selectedEmailHandler;
            }
            set
            {
                _selectedEmailHandler = value;
                NotifyOfPropertyChange(() => SelectedEmailHandler);
            }
        }

        public OpenFileResult<FileInfo> OpenFile()
        {
            OpenFileResult<FileInfo> openFileResult =
                OpenFileResult.OneFile("Please Choose the data File.")
                    .FilterFiles("CSV Files (*.csv)|*.csv");
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
            IEmailManager emailManager = _emailManagers.First(e => e.Metadata.Handler == SelectedEmailHandler).Value;

            emailManager.Send(GetStudentEmails(emailManager));
        }

        public bool CanStart(string dataFile)
        {
            return File.Exists(dataFile);
        }

        private IEnumerable<MailMessage> GetStudentEmails(IEmailManager emailManager)
        {
            IEnumerable<Student> students = _studentManager.LoadAll(_dataFile);

            foreach (Student student in students)
            {
                Requirements studentRequirements = _studentManager.DetermineRequirements(student.Courses);

                Requirements emailRequirements = _studentManager.DetermineEmails(student, studentRequirements, _cutOff);

                if (studentRequirements.HasFlag(Requirements.Practicum))
                {
                    emailRequirements |= Requirements.Practicum;
                }

                if (emailRequirements != Requirements.None && emailRequirements != Requirements.Practicum)
                {
                    yield return emailManager.GenerateEmail(student, emailRequirements);
                }
            }
        }
    }
}