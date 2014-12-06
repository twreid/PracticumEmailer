using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PracticumEmailer.Ui.ViewModels
{
    [Export(typeof (IShell))]
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