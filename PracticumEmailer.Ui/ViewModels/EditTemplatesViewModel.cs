using System.IO.Compression;
using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Caliburn.Micro.Extras;

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

        private ILog _logger = LogManager.GetLog(typeof (EditTemplatesViewModel));

        [ImportingConstructor]
        public EditTemplatesViewModel()
        {
            var info = new DirectoryInfo(_templatesPath);
            _files =
                new BindableCollection<string>(
                    info.EnumerateFiles("*.html").Select(fi => fi.Name.Remove(fi.Name.IndexOf('.')).ToUpper()));
        }

        public BindableCollection<String> Files { get { return _files; } }

        public String BindingContent
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

        public void ExportTemplates()
        {
            if (Directory.Exists(_templatesPath))
            {
                ZipFile.CreateFromDirectory(_templatesPath,
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        string.Format("templates-{0}.zip", DateTime.Now.ToFileTime())),
                        CompressionLevel.Optimal,
                        true);
            }
        }

        public IResult<FileInfo> ImportTemplates()
        {
            OpenFileResult<FileInfo> openFileResult =
                OpenFileResult.OneFile("Please file to import.")
                .FilterFiles("Zip Files (*.zip)|*.zip");

            openFileResult.Completed += (sender, args) =>
            {
                var openFile = sender as OpenFileResult<FileInfo>;
                if (openFile != null && openFile.Result.Exists)
                {
                    Console.WriteLine(openFile.Result.FullName);
                }
            };

            return openFileResult;
        }
    }
}
