using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContactList.Services
{
    public interface INavigationPagesService
    {
        Task NavigationPage(Page page, bool animation = true);
        Task PopPage( bool animation = false);
    }
}
