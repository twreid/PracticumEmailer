using Caliburn.Micro;
using PracticumEmailer.Ui.Properties;
using System;
using System.IO;
using System.IO.Compression;

namespace PracticumEmailer.Ui.IResults
{
    public class ImportResult : IResult
    {
        private readonly string _importFile;

        private readonly DirectoryInfo _templatesDirectory =
            new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.TemplateDirectory));

        public ImportResult(string importFile)
        {
            _importFile = importFile;
        }

        public void Execute(CoroutineExecutionContext context)
        {
            if (_templatesDirectory.Parent != null)
            {
                using (ZipArchive archive = ZipFile.OpenRead(_importFile))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(_templatesDirectory.FullName, entry.Name), true);
                    }
                }

            }

            Completed(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}