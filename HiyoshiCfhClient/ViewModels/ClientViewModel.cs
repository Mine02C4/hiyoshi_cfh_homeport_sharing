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

        #region Token変更通知プロパティ
        private string _Token;

        public string Token
        {
            get
            { return _Token; }
            set
            {
                if (_Token == value)
                    return;
                _Token = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public async void OpenLoginWindow()
        {
            using (var vm = new LoginViewModel())
            {
                var message = new TransitionMessage(vm, "Show/LoginWindow");
                Messenger.RaiseAsync(message);
                Token = vm.AccessToken;
            }
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
