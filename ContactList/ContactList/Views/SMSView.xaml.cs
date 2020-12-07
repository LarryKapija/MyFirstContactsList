using ContactList.ViewModels;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;
using ContactList.Models;
using Xamarin.Forms;

namespace ContactList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SMSView
    {
        public Person Contact { get; set; }
        public SMSView(Person contact)
        {
            this.Contact = contact;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(()=> 
                {
                    Sms.ComposeAsync(new SmsMessage(EntryMessage.Text, Contact.PhoneNumber));
                    PopupNavigation.Instance.PopAsync();
                });
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }

}