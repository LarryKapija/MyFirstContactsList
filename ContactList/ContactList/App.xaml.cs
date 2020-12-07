using ContactList.Pages;
using Todo;
using Xamarin.Forms;

namespace ContactList
{
    public partial class App : Application
    {

        static ContactListDataBase database;
        public App()
        {
            InitializeComponent();

            MainPage = new ContacstListPage();
        }

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

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
