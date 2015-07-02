using Livet;
using Livet.Messaging;
using System;
using System.Threading.Tasks;

namespace HiyoshiCfhClient.ViewModels
{
    class ClientViewModel : ViewModel
    {
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

        #region AccessToken変更通知プロパティ
        private string _AccessToken;

        public string AccessToken
        {
            get
            { return _AccessToken; }
            set
            {
                if (_AccessToken == value)
                    return;
                _AccessToken = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TokenType変更通知プロパティ
        private string _TokenType;

        public string TokenType
        {
            get
            { return _TokenType; }
            set
            {
                if (_TokenType == value)
                    return;
                _TokenType = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        Client Client;

        public async void OpenLoginWindow()
        {
            using (var vm = new LoginViewModel())
            {
                vm.PropertyChanged += vm_PropertyChanged;
                var message = new TransitionMessage(vm, "Show/LoginWindow");
                await Messenger.RaiseAsync(message);
            }
        }

        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "AccessToken":
                    AccessToken = (sender as LoginViewModel).AccessToken;
                    break;
                case "TokenType":
                    TokenType = (sender as LoginViewModel).TokenType;
                    break;
                default:
                    break;
            }
        }

        public async void Send()
        {
            // TODO: 送信処理の実装
            Client client = new Client(TokenType, AccessToken);
            DebugConsole += await client.CollectShipsData();
        }

        /// <summary>
        /// プラグインの起動シーケンスのテスト。
        /// </summary>
        public void StartTest()
        {
            if (CheckToken())
            {
                InitClient();
            }
            else
            {
                this.PropertyChanged += HandleLogin;
                OpenLoginWindow();
            }
        }

        void HandleLogin(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (CheckToken())
            {
                this.PropertyChanged -= HandleLogin;
                InitClient();
            }
        }

        void InitClient()
        {
            if (CheckToken())
            {
                try
                {
                    Task.Factory.StartNew(async () =>
                    {
                        OutDebugConsole("Start init client thread");
                        Client = new Client(TokenType, AccessToken, OutDebugConsole);
                        await Client.InitAdmiralInformation();
                        await Client.UpdateMasterData();
                        await Client.UpdateShips();
                        OutDebugConsole("End init client thread");
                    });
                }
                catch (Exception ex)
                {
                    OutDebugConsole(ex.ToString());
                }
            }
        }

        public async void GetShipTypes()
        {
            Client client = new Client(TokenType, AccessToken);
            DebugConsole += await client.GetShipTypes();
        }

        public void ClearConsole()
        {
            DebugConsole = "";
        }

        public async void SendShipTypes()
        {
            Client client = new Client(TokenType, AccessToken);
            try
            {
                await client.SendShipTypes();
            }
            catch (Exception ex)
            {
                OutDebugConsole(ex.ToString());
            }
        }

        public async void SendShipInfoes()
        {
            Client client = new Client(TokenType, AccessToken);
            try
            {
                await client.SendShipInfoes();
            }
            catch (Exception ex)
            {
                OutDebugConsole(ex.ToString());
            }
        }

        public async void RegisterAdmiral()
        {
            Client client = new Client(TokenType, AccessToken);
            try
            {
                await client.RegisterAdmiral();
            }
            catch (Exception ex)
            {
                OutDebugConsole(ex.ToString());
            }
        }

        private bool CheckToken()
        {
            return AccessToken != null && AccessToken.Length > 0 && TokenType != null && TokenType.Length > 0;
        }

        private void OutDebugConsole(string msg)
        {
            DebugConsole += msg + System.Environment.NewLine;
        }
    }
}
