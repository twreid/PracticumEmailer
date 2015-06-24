using Caliburn.Micro;
using PracticumEmailer.Ui.Properties;
using System;
using System.IO;
using System.IO.Compression;

namespace PracticumEmailer.Ui.IResults
{
    public class ExportResult : IResult
    {
        private readonly DirectoryInfo _templatesDirectory =
            new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.TemplateDirectory));

        public void Execute(CoroutineExecutionContext context)
        {
            if (_templatesDirectory.Exists)
            {
                ZipFile.CreateFromDirectory(_templatesDirectory.FullName,
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        string.Format("templates-{0}.zip", DateTime.Now.ToFileTime())),
                    CompressionLevel.Optimal,
                    true);
            }

            Completed(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}