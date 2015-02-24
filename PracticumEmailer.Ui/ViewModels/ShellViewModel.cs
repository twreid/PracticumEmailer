using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace PracticumEmailer.Ui.ViewModels
{
    [Export(typeof (IShell))]
    public class ShellViewModel : Conductor<Screen>, IShell
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ShellViewModel()
        {
            ShowEmailManager();
            DisplayName = "Practicum Emailer";
        }

        public void ShowEmailManager()
        {
            ActivateItem(IoC.Get<MainScreenViewModel>());
        }

        public void ShowEditClearances()
        {
            ActivateItem(IoC.Get<EditClearancesViewModel>());
        }

        public void ShowEditTemplates()
        {
            ActivateItem(IoC.Get<EditTemplatesViewModel>());
        }
    }
}