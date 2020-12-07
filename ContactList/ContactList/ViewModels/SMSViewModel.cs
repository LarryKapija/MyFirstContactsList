using Rg.Plugins.Popup.Services;
using System.Windows.Input;

namespace ContactList.ViewModels
{
    class SMSViewModel
    {
        public SMSViewModel()
        {
            PopupNavigation.Instance.PopAsync();
        }

        public ICommand CancelCommand { get; set; }

        public ICommand SendMessage { get; set; }
    }
}
