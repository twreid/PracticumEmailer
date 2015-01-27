﻿using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace PracticumEmailer.Ui.ViewModels
{

    public class EditTemplatesViewModel : Screen
    {
        private readonly string _templatesPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Properties.Settings.Default.TemplateDirectory);
        private readonly BindableCollection<string> _files;

        private string _currentTemplate;
        private string _currentContent;

        [ImportingConstructor]
        public EditTemplatesViewModel()
        {
            var info = new DirectoryInfo(_templatesPath);
            _files =
                new BindableCollection<string>(
                    info.EnumerateFiles("*.html").Select(fi => fi.Name.Remove(fi.Name.IndexOf('.')).ToUpper()));
        }

        public BindableCollection<String> Files { get { return _files; } }

        public String ContentHtml
        {
            get
            {
                return _currentContent;
            }
            set
            {
                _currentContent = value;
                NotifyOfPropertyChange(() => ContentHtml);
            }
        }

        public void OnSelectionChanged(string file)
        {
            _currentTemplate = Path.Combine(_templatesPath, string.Format("{0}.html", file.ToLower()));
            ContentHtml = File.ReadAllText(_currentTemplate);
        }
    }
}