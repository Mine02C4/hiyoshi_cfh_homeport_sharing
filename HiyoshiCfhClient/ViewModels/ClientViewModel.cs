using Livet;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiyoshiCfhClient.ViewModels
{
    class ClientViewModel : ViewModel
    {
        public void OpenLoginWindow()
        {
            var message = new TransitionMessage("Show/LoginWindow");
            this.Messenger.RaiseAsync(message);
        }
    }
}
