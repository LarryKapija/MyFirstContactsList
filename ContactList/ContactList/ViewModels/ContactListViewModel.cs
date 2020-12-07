using ContactList.Models;
using ContactList.Pages;
using ContactList.Services;
using MyFirstMVVM.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Todo;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ContactList.ViewModels
{
    public class ContactListViewModel: BaseViewModel 
    {
        public bool IsRefreshing = false;
        static ContactListDataBase database;
        public static ContactListDataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ContactListDataBase();
                }
                return database;
            }
        }
        

        Person selectedContact;
        public Person SelectedContact
        {
            get { return selectedContact; }
            set
            {
                if(value != null)
                {
                    selectedContact = value;
                    ContactProfile(selectedContact);
                }

            }
        }


        public ObservableCollection<Person> Contact { get; set; } = new ObservableCollection<Person>();

        public ContactListViewModel(INavigationPagesService navigationPageService, IDisplayDialogService displayDialogService)
        {
            this._navigationPageService = navigationPageService;
            this._displayDialogService = displayDialogService;
            

            DeleteContactCommand = new Command<Person>(async tappedContact => 
            {
                string action, delete, cancell;
                delete = "Eliminar";
                cancell = "Cancelar";
                action = await App.Current.MainPage.DisplayActionSheet("Desea eliminar este contacto?", $"{cancell}", $"{delete}" );

                if (action == delete)
                {
                    await Database.DeleteItemAsync(tappedContact);
                    Contact.Remove(tappedContact);
                    IsRefreshing = true;
                    await _displayDialogService.DisplayMessage("ELIMINADO", "Contacto  eliminado", "OK");

                }

    
            });
        }

        #region INTERFACES
        INavigationPagesService _navigationPageService;
        IDisplayDialogService _displayDialogService;
        #endregion



        public async void ContactProfile(Person selectedContact)
        {
            await _navigationPageService.NavigationPage(new ContactProfilePage(Contact,selectedContact),true);

        }

        
        #region COLORSELECTOR :P

        string[] colors = { "#e6261f", "#eb7532", "#f7d038", "#a3e048", "#49da9a", "#34bbe6", "#4355db", "#d23be7" };
        Random randomNumber => new Random();
        int index => randomNumber.Next(0, colors.Length);
        public string Color => colors[index];

        #endregion

        #region COMMANDS
        public ICommand AddContactCommand => new Xamarin.Forms.Command(async () =>
        {
            await _navigationPageService.NavigationPage(new AddContactFormPage(Contact),true);
        });
        public ICommand DeleteContactCommand { get; set; }

            #endregion
        }
}
