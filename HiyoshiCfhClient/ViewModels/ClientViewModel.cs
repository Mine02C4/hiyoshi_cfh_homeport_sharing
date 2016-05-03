using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using HiyoshiCfhClient.Models;
using HiyoshiCfhClient.Utils;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;
using Microsoft.OData.Client;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialType = HiyoshiCfhClient.HiyoshiCfhWeb.Models.MaterialType;

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
        bool setHandleLogin = false;
        PropertyChangedEventListener OrganizationListener = null;
        PropertyChangedEventListener ItemyardListener = null;
        Client Client = null;
        QuestsTracker QuestsTracker;

        public ClientViewModel()
        {
            OutDebugConsole("Initialize");
            Settings.Init();
            QuestsTracker = new QuestsTracker();
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
            if (OrganizationListener == null || ItemyardListener == null)
            {
                #region 艦娘の変更検知
                OrganizationListener = new PropertyChangedEventListener(KanColleClient.Current.Homeport.Organization);
                OrganizationListener.RegisterHandler(() => KanColleClient.Current.Homeport.Organization.Ships,
                async (s, h) =>
                {
                    try
                    {
                        OutDebugConsole("Handle Change of ships");
                        if (CheckToken() && EnableAutoUpdate && KanColleClient.Current.Homeport.Organization.Ships.Count > 0)
                        {
                            await PrepareClient();
                            await Client.UpdateShips();
                            await Client.UpdateSlotItems();
                        }
                    }
                    catch (DeniedAccessToAdmiral)
                    {
                        OutDebugConsole("認証に失敗しました。再認証が必要です。");
                        ReLogin();
                    }
                    catch (Exception ex)
                    {
                        OutDebugConsole(ex.ToString());
                    }
                });
                this.CompositeDisposable.Add(OrganizationListener);
                #endregion
                #region 装備の変更検知
                ItemyardListener = new PropertyChangedEventListener(KanColleClient.Current.Homeport.Itemyard);
                ItemyardListener.RegisterHandler(() => KanColleClient.Current.Homeport.Itemyard.SlotItems,
                async (s, h) =>
                {
                    try
                    {
                        OutDebugConsole("Handle Change of SlotItems");
                        if (CheckToken() && EnableAutoUpdate && KanColleClient.Current.Homeport.Itemyard.SlotItems.Count > 0)
                        {
                            await PrepareClient();
                            await Client.UpdateSlotItems();
                        }
                    }
                    catch (Exception ex)
                    {
                        OutDebugConsole(ex.ToString());
                    }
                });
                this.CompositeDisposable.Add(ItemyardListener);
                #endregion
                #region 任務の取得検知
                var proxy = KanColleClient.Current.Proxy;
                proxy.api_get_member_questlist
                    .Select(QuestsTracker.QuestListSerialize)
                    .Where(x => x != null)
                    .Subscribe(async x => { await this.HandleQuests(x); });
                #endregion
                #region 資材の変更検知
                KanColleClient.Current.Homeport.Materials.PropertyChanged += MaterialsChanged;
                #endregion
                IsInited = true;
            }
        }

        private async void MaterialsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var materials = sender as Materials;
            OutDebugConsole("Handle material: " + e.PropertyName);
            OutDebugConsole(string.Format("Fuel: {0} Bull: {1} Steel: {2} Bauxite: {3}",
                materials.Fuel, materials.Ammunition, materials.Steel, materials.Bauxite));
            try
            {
                await PrepareClient();
                switch (e.PropertyName)
                {
                    case "Fuel":
                        await Client.AddMaterialRecord(MaterialType.Fuel, materials.Fuel);
                        break;
                    case "Ammunition":
                        await Client.AddMaterialRecord(MaterialType.Bull, materials.Ammunition);
                        break;
                    case "Steel":
                        await Client.AddMaterialRecord(MaterialType.Steel, materials.Steel);
                        break;
                    case "Bauxite":
                        await Client.AddMaterialRecord(MaterialType.Bauxite, materials.Bauxite);
                        break;
                    case "InstantBuildMaterials":
                        await Client.AddMaterialRecord(MaterialType.InstantBuildMaterials, materials.InstantBuildMaterials);
                        break;
                    case "InstantRepairMaterials":
                        await Client.AddMaterialRecord(MaterialType.InstantRepairMaterials, materials.InstantRepairMaterials);
                        break;
                    case "DevelopmentMaterials":
                        await Client.AddMaterialRecord(MaterialType.DevelopmentMaterials, materials.DevelopmentMaterials);
                        break;
                    case "ImprovementMaterials":
                        await Client.AddMaterialRecord(MaterialType.RenovationMaterials, materials.ImprovementMaterials);
                        break;
                    default:
                        break;
                }
            }
            catch (DeniedAccessToAdmiral)
            {
                OutDebugConsole("認証に失敗しました。再認証が必要です。");
                ReLogin();
            }
            catch (AdmiralNotInitialized)
            {
                OutDebugConsole("認証が必要です。");
                ReLogin();
            }
            catch (Exception ex)
            {
                OutDebugConsole(ex.ToString());
            }

        }

        async Task HandleQuests(kcsapi_questlist questlist)
        {
            await Task.Factory.StartNew(async () =>
            {
                OutDebugConsole("Handle questlist");
                try
                {
                    QuestsTracker.AddPage(questlist);
                    if (QuestsTracker.IsIntegral)
                    {
                        await PrepareClient();
                        await Client.UpdateQuests(QuestsTracker.DisplayedQuests);
                    }
                }
                catch (DeniedAccessToAdmiral)
                {
                    OutDebugConsole("認証に失敗しました。再認証が必要です。");
                    ReLogin();
                }
                catch (Exception ex)
                {
                    OutDebugConsole(ex.ToString());
                }
            });
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
                StartLogin();
            }
        }

        /// <summary>
        /// ログインシーケンの開始
        /// </summary>
        private void StartLogin()
        {
            if (!setHandleLogin)
            {
                this.PropertyChanged += HandleLogin;
                setHandleLogin = true;
            }
            OpenLoginWindow();
        }

        void HandleLogin(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (CheckToken())
            {
                this.PropertyChanged -= HandleLogin;
                setHandleLogin = false;
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
                if (TokenType == null || AccessToken == null)
                {
                    throw new AdmiralNotInitialized();
                }
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
                        await Client.UpdateSlotItems();
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
            Settings.Current.AccessToken = null;
            Settings.Current.TokenType = null;
            Settings.Current.Save();
            Client = null;
        }

        bool CheckToken()
        {
            return AccessToken != null && AccessToken.Length > 0 && TokenType != null && TokenType.Length > 0;
        }

        void ReLogin()
        {
            try
            {
                ClearToken();
                Client = null;
                StartLogin();
            }
            catch (Exception ex)
            {
                OutDebugConsole(ex.ToString());
            }
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
