using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using HiyoshiCfhClient.Models;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
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

        #region EnableAutoUpdate変更通知プロパティ
        private bool _EnableAutoUpdate;

        public bool EnableAutoUpdate
        {
            get
            { return _EnableAutoUpdate; }
            set
            {
                if (_EnableAutoUpdate == value)
                    return;
                _EnableAutoUpdate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        bool IsInited = false;

        PropertyChangedEventListener AutoUpdateToggleListener = null;

        Client Client = null;

        public ClientViewModel()
        {
            OutDebugConsole("Initialize");
            Settings.Init();
            AccessToken = Settings.Current.AccessToken;
            TokenType = Settings.Current.TokenType;
            EnableAutoUpdate = true;
            var kccListener = new PropertyChangedEventListener(KanColleClient.Current);
            kccListener.RegisterHandler(() => KanColleClient.Current.IsStarted, (_, __) =>
            {
                if (!IsInited && KanColleClient.Current.IsStarted)
                {
                    InitHandlers();
                }
            });
            this.CompositeDisposable.Add(kccListener);
        }

        void InitHandlers()
        {
            if (AutoUpdateToggleListener == null)
            {
                AutoUpdateToggleListener = new PropertyChangedEventListener(KanColleClient.Current.Homeport.Organization);
                AutoUpdateToggleListener.RegisterHandler(() => KanColleClient.Current.Homeport.Organization.Ships,
                async (s, h) =>
                {
                    try
                    {
                        OutDebugConsole("Handle Change of ships");
                        if (CheckToken() && EnableAutoUpdate && KanColleClient.Current.Homeport.Organization.Ships.Count > 0)
                        {
                            await PrepareClient();
                            await Client.UpdateShips();
                        }
                    }
                    catch (Exception ex)
                    {
                        OutDebugConsole(ex.ToString());
                    }
                });
                this.CompositeDisposable.Add(AutoUpdateToggleListener);
                IsInited = true;
            }
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
                Settings.Current.AccessToken = AccessToken;
                Settings.Current.TokenType = TokenType;
                Settings.Current.Save();
                InitClient();
            }
        }

        async Task PrepareClient()
        {
            if (Client == null)
            {
                Client = new Client(TokenType, AccessToken, OutDebugConsole);
                await Client.InitClientAsync();
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
                        await PrepareClient();
                        await Client.UpdateShips();
                        OutDebugConsole("End init client thread");
                    }
                    catch (DeniedAccessToAdmiral)
                    {
                        OutDebugConsole("提督情報の変更が拒否されました。ログインアカウントが異なります。");
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

        void ClearToken()
        {
            AccessToken = null;
            TokenType = null;
            Client = null;
        }

        bool CheckToken()
        {
            return AccessToken != null && AccessToken.Length > 0 && TokenType != null && TokenType.Length > 0;
        }

        private static string logPath = Path.Combine(
            Environment.CurrentDirectory,
            "HiyoshiCfhClient.log");

        private void OutDebugConsole(string msg)
        {
            var log = DateTime.Now.ToString("O") + " : " + msg + System.Environment.NewLine;
            DebugConsole += log;
            Task.Factory.StartNew(async () =>
            {
                StreamWriter stream = null;

                try
                {
                    stream = new StreamWriter(logPath, true, new UTF8Encoding(false));
                    await stream.WriteAsync(log);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            });
        }
    }
}
