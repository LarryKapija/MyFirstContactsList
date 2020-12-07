using ContactList.Models;
using ContactList.Services;
using ContactList.ViewModels;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactProfilePage : ContentPage
    {
        public ContactProfilePage(ObservableCollection<Person> contact, Person selectedContact)
        {
            InitializeComponent();
            BindingContext = new ContactProfileViewModel(contact, selectedContact, new NavigationPagesService(),new DisplayDialogService());
        }
    }
}