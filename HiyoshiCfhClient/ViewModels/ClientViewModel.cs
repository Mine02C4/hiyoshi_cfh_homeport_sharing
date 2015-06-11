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
        public string BaseUri { get; set; }

        #region DebugConsole変更通知プロパティ
        private string _DebugConsole;

        public string DebugConsole
        {
            get
            { return _DebugConsole; }
            set
            {
                if (_DebugConsole == value)
                    return;
                _DebugConsole = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public void OpenLoginWindow()
        {
            var message = new TransitionMessage("Show/LoginWindow");
            this.Messenger.RaiseAsync(message);
        }

        public async void Send()
        {
            // TODO: 送信処理の実装
            Client client = new Client();
            DebugConsole += await client.CollectShipsData();
        }

        public async void GetShipTypes()
        {
            Client client = new Client();
            DebugConsole += await client.GetShipTypes();
        }
    }
}
