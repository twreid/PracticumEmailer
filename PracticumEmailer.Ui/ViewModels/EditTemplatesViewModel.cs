using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace PracticumEmailer.Ui.ViewModels
{

    public class EditTemplatesViewModel : Screen
    {
        private readonly string _templatesPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Properties.Settings.Default.TemplateDirectory);
        private readonly BindableCollection<string> _files;

        [ImportingConstructor]
        public EditTemplatesViewModel()
        {
            var info = new DirectoryInfo(_templatesPath);
            _files =
                new BindableCollection<string>(
                    info.EnumerateFiles("*.html").Select(fi => fi.Name.Remove(fi.Name.IndexOf('.')).ToUpper()));
        }

        public BindableCollection<String> Files { get { return _files; } }
    }
}
