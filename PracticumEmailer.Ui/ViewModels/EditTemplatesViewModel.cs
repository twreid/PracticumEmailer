using Caliburn.Micro;
using Caliburn.Micro.Extras;
using PracticumEmailer.Ui.IResults;
using PracticumEmailer.Ui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace PracticumEmailer.Ui.ViewModels
{
    public class EditTemplatesViewModel : Screen
    {
        private readonly BindableCollection<string> _files;

        private readonly string _templatesPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.TemplateDirectory);

        private string _currentContent;
        private string _currentTemplate;

        private ILog _logger = LogManager.GetLog(typeof (EditTemplatesViewModel));

        [ImportingConstructor]
        public EditTemplatesViewModel()
        {
            var info = new DirectoryInfo(_templatesPath);
            _files =
                new BindableCollection<string>(
                    info.EnumerateFiles("*.html").Select(fi => fi.Name.Remove(fi.Name.IndexOf('.')).ToUpper()));
        }

        public BindableCollection<string> Files
        {
            get { return _files; }
        }

        public string BindingContent
        {
            get
            {
                return _currentContent;
            }
            set
            {
                _currentContent = value;
                NotifyOfPropertyChange(() => BindingContent);
            }
        }

        public void OnSelectionChanged(string file)
        {
            _currentTemplate = Path.Combine(_templatesPath, string.Format("{0}.html", file.ToLower()));
            BindingContent = File.ReadAllText(_currentTemplate);
        }

        public void Save()
        {
            File.WriteAllText(_currentTemplate, _currentContent);
        }

        public IResult ExportTemplates()
        {
            return new ExportResult();
        }

        public IEnumerable<IResult> ImportTemplates()
        {
            OpenFileResult<FileInfo> openFileResult =
                OpenFileResult.OneFile("Please file to import.")
                    .FilterFiles("Zip Files (*.zip)|*.zip");
            string file = string.Empty;

            openFileResult.Completed += (sender, args) =>
            {
                var openFile = sender as OpenFileResult<FileInfo>;
                if (openFile != null && openFile.Result.Exists)
                {
                    file = openFile.Result.FullName;
                }
            };

            yield return openFileResult;
            yield return new ImportResult(file);
        }
    }
}