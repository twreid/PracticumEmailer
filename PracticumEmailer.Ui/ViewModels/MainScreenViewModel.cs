﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Caliburn.Micro.Extras;

namespace PracticumEmailer.Ui.ViewModels
{
    public class MainScreenViewModel : Screen
    {
        private string _displayName;
        private string _dataFile;

        private DateTime _cutOff;

        public MainScreenViewModel()
        {
            _displayName = "Practicum Emailer";
            _cutOff = DateTime.Now;
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
            var openFileResult =
                OpenFileResult.OneFile("Please Choose the data File.").FilterFiles("CSV Files (*.csv)|*.csv");
            openFileResult.Completed += (sender, args) =>
            {
                var openFile = sender as OpenFileResult<FileInfo>;
                DataFile = openFile.Result.FullName;
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
            MessageBox.Show(string.Format("Click {0}", dataFile));
        }

        public bool CanStart(string dataFile)
        {
            return File.Exists(dataFile);
        }
    }
}