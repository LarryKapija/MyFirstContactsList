using ContactList.Models;
using ContactList.Services;
using ContactList.ViewModels;
using System.Collections.ObjectModel;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddContactFormPage : ContentPage
    {
        public AddContactFormPage(ObservableCollection<Person> contact)
        {
            InitializeComponent();
            BindingContext = new AddContactFormViewModel(contact, new DisplayDialogService(),new NavigationPagesService());
        }   
        public AddContactFormPage(ObservableCollection<Person> contact, Person selectedContact)
        {
            InitializeComponent();
            BindingContext = new AddContactFormViewModel( contact,selectedContact, new DisplayDialogService(), new NavigationPagesService());
        }
    }
}