using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContactList.Services
{
    public class NavigationPagesService : INavigationPagesService
    {
        public async Task NavigationPage(Page page, bool animation = true)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(page, animation);
        }

        public async Task PopPage(bool animation = false)
        {
            await App.Current.MainPage.Navigation.PopModalAsync(animation);

        }
    }
}
