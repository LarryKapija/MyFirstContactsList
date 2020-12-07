using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactList.Services
{
    class DisplayDialogService : IDisplayDialogService
    {

        public async Task DisplayMessage(string title, string description, string okText = "OK")
        {
            await App.Current.MainPage.DisplayAlert(title, description, okText);
        }
    }
}
