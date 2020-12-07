using ContactList.Models;
using ContactList.Pages;
using ContactList.Services;
using ContactList.Views;
using MyFirstMVVM.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ContactList.ViewModels
{
    internal class ContactProfileViewModel :BaseViewModel
    {
        private ObservableCollection<Person> contact;
        private Person SelectedContact { get; set; }

        INavigationPagesService _navigationPagesService;
        IDisplayDialogService _displayDialogService;

        public ContactProfileViewModel(ObservableCollection<Person> contact, Person selectedContact, INavigationPagesService navigationPagesService,IDisplayDialogService displayDialogService)
        {
            this.contact = contact;
            this.SelectedContact = selectedContact;
            this._navigationPagesService = navigationPagesService;
            this._displayDialogService = displayDialogService;

            EditContactInformation  = new Command(async () =>
            {
                await _navigationPagesService.NavigationPage(new AddContactFormPage(contact, SelectedContact), true);
            });

            CallCommand = new Command(async() =>
            {
                await PlacePhoneCall(PhoneNumber);
            });

            MessageCommand = new Command(async()=> 
            {
                await PlaceMessage();
            
            });

            VideoCallCommand = new Command(async()=> 
            {
                await PlaceVideoCall();
            });
        }


        public ICommand EditContactInformation { get; set; }

        public ICommand CallCommand { get; set; }
        public ICommand MessageCommand { get; set; }
        public ICommand VideoCallCommand { get; set; }

        async Task PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException)
            {
                // Number was null or white space
                await _displayDialogService.DisplayMessage("ERROR", "Error 001 \nFavor contactar con soporte 888-888-8888", "Ok");
            }
            catch (FeatureNotSupportedException)
            {
                // Phone Dialer is not supported on this device.
                await _displayDialogService.DisplayMessage("ERROR", "Error number 002 \nFavor contactar con soporte 888-888-8888", "Ok");
            }
            catch (Exception)
            {
                // Other error has occurred.
                await _displayDialogService.DisplayMessage("UNKNOW ERROR", "Favor contactar con soporte 888-888-8888", "Ok");
            }
        }

        async Task PlaceMessage()
        {
            await PopupNavigation.Instance.PushAsync(new SMSView(SelectedContact),true);
        }

        async Task PlaceVideoCall()
        {
           await _displayDialogService.DisplayMessage("OOPS","Esta funcion aun no esta disponible.\nFavor esperar futuras actualizaciones","Ok");
        }

        public string Name => SelectedContact.Name;
        public string LastName => SelectedContact.LastName;
        public string Company => SelectedContact.Company;
        public string PhoneNumber => SelectedContact.PhoneNumber;
        public string Email => SelectedContact.Email; 
        public string ProfilePhoto => SelectedContact.ProfilePhoto;
    }
}