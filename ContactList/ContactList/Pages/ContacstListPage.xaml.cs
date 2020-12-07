using ContactList.Services;
using ContactList.ViewModels;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContacstListPage : ContentPage
    {
        public ContacstListPage()
        {
            InitializeComponent();
            BindingContext =  new ContactListViewModel(new NavigationPagesService(), new DisplayDialogService());
        }


        ListView ContactsList;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetItemsAsync();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string toLower = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.NewTextValue);
            var ContainerSearch = BindingContext as ContactListViewModel;
            ContactsList.BeginRefresh();
            if (string.IsNullOrWhiteSpace(toLower))
            {
                ContactsList.ItemsSource = ContainerSearch.Contact;
            }
            else
            {
                ContactsList.ItemsSource = ContainerSearch.Contact.Where(s => s.Name.Contains(toLower));
            }
            ContactsList.EndRefresh();
            SearchBar searchBar = (SearchBar)sender;
        }
    }
}