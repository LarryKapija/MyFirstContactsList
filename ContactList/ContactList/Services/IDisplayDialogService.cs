using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactList.Services
{
    public interface IDisplayDialogService
    {

        Task DisplayMessage(string title, string description, string okText = "OK");
    }
}
