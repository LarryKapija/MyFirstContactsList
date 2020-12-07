using ContactList.Models;
using ContactList.Services;
using MyFirstMVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using Todo;
using ZXing.Net.Mobile.Forms;

namespace ContactList.ViewModels
{
    public class AddContactFormViewModel : BaseViewModel
    {
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

        Person EditableContact;
        public string QrContent;
        public string[] QrContentList;


        #region CONSTRUCTORS
        public AddContactFormViewModel(ObservableCollection<Person> contact, IDisplayDialogService displayDialogService, INavigationPagesService navigationPagesService)
        {
            this._displayDialog = displayDialogService;
            this._navigationPagesService = navigationPagesService;

            CreateContactCommand = new Command( async()=> 
            {
                if (string.IsNullOrEmpty(NameTxt))
                {
                    await _displayDialog.DisplayMessage("ERROR", "campo nombre no puede estar vacio","Ok");
                }
                else
                {
                    Contact = new Person { Name = NameTxt, 
                        LastName = LastNameTxt, 
                        Company = CompanyTxt, 
                        PhoneNumber = PhoneNumberTxt, 
                        Email = EmailTxt, 
                        ProfilePhoto = ProfilePhoto };

                    //Guardando contacto en la lista y la DB
                    contact.Add(Contact );
                    await Database.SaveItemAsync(Contact);
                    await _navigationPagesService.PopPage();
                }

            });

            AddProfilePhotoCommand = new Command(async()=>
            {
                string Action, TakePhoto, ChoosePhoto;
                TakePhoto = "Tomar foto";
                ChoosePhoto = "Elegir una foto";
                Action=  await App.Current.MainPage.DisplayActionSheet("Cambiar foto", "Cancelar", null, $"{TakePhoto}", $"{ChoosePhoto}");

                if(Action == TakePhoto)
                {
                    await TakePhotoAsync();
                }
                else if (Action == ChoosePhoto)
                {
                    await LoadPhotoAsync();
                }    
            });
            AddContactByQRCode = new Command(async () => 
            {
                await Scanner();
            });
        }

        public AddContactFormViewModel(ObservableCollection<Person> contact, Person _selectedContact, IDisplayDialogService displayDialogService, INavigationPagesService navigationPagesService)
        {
            this._navigationPagesService = navigationPagesService;
            this.EditableContact =_selectedContact;
            this.name = EditableContact.Name;
            this.lastName = EditableContact.LastName;
            this.company = EditableContact.Company;
            this.phoneNumber = EditableContact.PhoneNumber;
            this.email = EditableContact.Email;
            this.profilePhoto = EditableContact.ProfilePhoto;

            CreateContactCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(NameTxt))
                {
                    await _displayDialog.DisplayMessage("ERROR", "campo nombre no puede estar vacio","Ok");
                }
                else
                {
                    Contact = new Person { Name = NameTxt, 
                        LastName = LastNameTxt, 
                        Company = CompanyTxt, 
                        PhoneNumber = PhoneNumberTxt, 
                        Email = EmailTxt, 
                        ProfilePhoto = ProfilePhoto };
                    //Eliminando contacto de la lista y la DB
                    contact.Remove(EditableContact);
                    await Database.DeleteItemAsync(EditableContact);
                    //Guardando contacto en la lista y la DB
                    contact.Add(Contact);
                    await Database.SaveItemAsync(Contact);


                    Device.BeginInvokeOnMainThread(()=> 
                    {
                        _navigationPagesService.PopPage(true);
                        _navigationPagesService.PopPage(true);
                    });
                }
            });

            AddProfilePhotoCommand = new Command(async () =>
            {
                string Action, TakePhoto, ChoosePhoto;
                TakePhoto = "Tomar foto";
                ChoosePhoto = "Elegir una foto";
                Action = await App.Current.MainPage.DisplayActionSheet("Cambiar foto", "Cancelar", null, $"{TakePhoto}", $"{ChoosePhoto}");

                if (Action == TakePhoto)
                {
                    await TakePhotoAsync();
                }
                else if (Action == ChoosePhoto)
                {
                    await LoadPhotoAsync();
                }
            });

            AddContactByQRCode = new Command(async () =>
            { 
                await Scanner();
            });

        }

        #endregion
      
        #region COMMANDS

        public ICommand CreateContactCommand { get; }

        public ICommand AddProfilePhotoCommand { get; set; }

        public ICommand AddContactByQRCode { get; set; }

        #endregion

        #region INTERFACES

        IDisplayDialogService _displayDialog;

        INavigationPagesService _navigationPagesService;

        #endregion

        #region TASKS

        async Task TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _displayDialog.DisplayMessage("ERROR", "Dispositivo no tiene camara disponible", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Contacts profile Photos",
                Name = $"{CompleteName}.jpg",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
            });

            var stream = file.GetStream();

            if (file == null)
                return;
            else
                ProfilePhoto = file.Path; 
        }

        async Task LoadPhotoAsync()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await _displayDialog.DisplayMessage("ERROR", "No tiene permiso para acceder a la galeria", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });

            if (file == null) 
            {
                return;
            }
            else 
            {
                ProfilePhoto = file.Path;
            }            
        }

        async Task Scanner()
        {
            var scannerPage = new ZXingScannerPage();

            scannerPage.Title = "Lector de QR";
            scannerPage.OnScanResult += (results) =>
            {
                scannerPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    _navigationPagesService.PopPage(true);

                    NameTxt = results.Text.Split('*')[0];
                    LastNameTxt = results.Text.Split('*')[1];
                    PhoneNumberTxt = results.Text.Split('*')[2];
                });
            };
            await _navigationPagesService.NavigationPage(scannerPage);
        }
        #endregion

        #region ATTRIBUTES
        public string name;
        public string lastName;
        public string company;
        public string phoneNumber;
        public string email;
        public string profilePhoto;
        #endregion

        #region PROPERTIES

        private string CompleteName => NameTxt + " " + LastNameTxt;
       
        public string? NameTxt
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }
        public string? LastNameTxt
        {
            get { return this.lastName; }
            set { SetValue(ref this.lastName, value); }
        }
        public string? CompanyTxt
        {
            get { return this.company; }
            set { SetValue(ref this.company, value); }
        }
        public string PhoneNumberTxt
        {
            get { return this.phoneNumber; }
            set { SetValue(ref this.phoneNumber, value); }
        }
        public string? EmailTxt
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public Person Contact { get;set; }

        public string? ProfilePhoto = "DefaultProfilePhoto";
        #endregion

    }

}
