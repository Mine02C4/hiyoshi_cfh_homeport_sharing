using HiyoshiCfhClient.Models;
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

        public ClientViewModel()
        {
            OutDebugConsole("Initialize");
            Settings.Init();
            AccessToken = Settings.Current.AccessToken;
            TokenType = Settings.Current.TokenType;
        }

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
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        OutDebugConsole("Start init client thread");
                        Settings.Current.AccessToken = AccessToken;
                        Settings.Current.TokenType = TokenType;
                        Settings.Current.Save();
                        Client = new Client(TokenType, AccessToken, OutDebugConsole);
                        await Client.InitAdmiralInformation();
                        await Client.UpdateMasterData();
                        await Client.UpdateShips();
                        OutDebugConsole("End init client thread");

                    }
                    catch (Exception ex)
                    {
                        OutDebugConsole(ex.ToString());
                    }
                });
            }
        }

        public void ClearConsole()
        {
            DebugConsole = "";
        }

        private bool CheckToken()
        {
            return AccessToken != null && AccessToken.Length > 0 && TokenType != null && TokenType.Length > 0;
        }

        private void OutDebugConsole(string msg)
        {
            DebugConsole += DateTime.Now.ToString("O") + " : " +  msg + System.Environment.NewLine;
        }
    }
}
