using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace PracticumEmailer.Ui.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive, IShell
    {
        public ShellViewModel()
        {
            ShowStartingScreen();
        }

        public void ShowStartingScreen()
        {
            ActivateItem(new MainScreenViewModel());
        }
    }
}
