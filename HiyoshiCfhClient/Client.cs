using Grabacr07.KanColleWrapper;
using HiyoshiCfhClient.Default;
using WebShipType = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipType;
using WebShipInfo = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipInfo;
using WebAdmiral = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Admiral;
using WebShip = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Ship;
using WebQuest = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;
using System.Collections.ObjectModel;
using Microsoft.OData.Client;
using System.Diagnostics;
using System.Threading;
using HiyoshiCfhClient.Utils;

namespace HiyoshiCfhClient
{
    class Client
    {
        Container Context;
        string TokenType;
        string AccessToken;
        WebAdmiral Admiral = null;
        public delegate void DebugConsole(string msg);
        DebugConsole _DebugConsole;
        IQueryable<WebShip> Ships = null;
        LimitedConcurrencyLevelTaskScheduler taskScheduler;
        TaskFactory factory;

        public Client(string tokenType, string accessToken)
        {
            TokenType = tokenType;
            AccessToken = accessToken;
            ResetContext();
            taskScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            factory = new TaskFactory(taskScheduler);
        }

        public Client(string tokenType, string accessToken, DebugConsole debugConsole)
            : this(tokenType, accessToken)
        {
            _DebugConsole = debugConsole;
        }

        private void ResetContext()
        {
            Context = new Container(new Uri("http://hiyoshicfhweb.azurewebsites.net/odata"));
            if (TokenType != null && AccessToken != null)
            {
                Context.SendingRequest2 += (sender, eventArgs) =>
                {
                    eventArgs.RequestMessage.SetHeader("Authorization", TokenType + " " + AccessToken);
                };
            }
            //Context.SaveChangesDefaultOptions = SaveChangesOptions.BatchWithSingleChangeset;
        }

        private void OutDebugConsole(string msg)
        {
            if (_DebugConsole != null)
            {
                _DebugConsole(msg);
            }
        }

        public async Task InitClientAsync()
        {
            await factory.StartNew(() =>
            {
                InitAdmiral();
                UpdateMasterData();
                InitShips();
            });
        }

        void InitShips()
        {
            CheckAdmiral();
            if (Ships == null)
            {
                Ships = Context.Ships.Where(x => x.AdmiralId == Admiral.AdmiralId);
            }
        }

        /// <summary>
        /// サーバーから提督情報を取得し、同期します。初回の場合は新規に追加し、アクセス拒否された場合は例外を投げます。
        /// </summary>
        void InitAdmiral()
        {
            OutDebugConsole("InitAdmiral");
            var memberId = int.Parse(KanColleClient.Current.Homeport.Admiral.MemberId);
            Admiral = GetAdmiral(memberId);
            // 新規の場合は登録処理
            if (Admiral == null)
            {
                var admiral = new WebAdmiral(KanColleClient.Current.Homeport.Admiral);
                Context.AddToAdmirals(admiral);
                Context.SaveChanges();
                Admiral = GetAdmiral(memberId);
            }
            else // 既存の場合は更新処理
            {
                UpdateAdmiral();
            }
        }

        void CheckAdmiral()
        {
            if (Admiral == null)
            {
                throw new AdmiralNotInitialized();
            }
        }

        WebAdmiral GetAdmiral(int memberId)
        {
            OutDebugConsole("GetAdmiral");
            return Context.Admirals.Where(x => x.MemberId == memberId).FirstOrDefault();
        }

        void UpdateAdmiral()
        {
            OutDebugConsole("UpdateAdmiral");
            var admiral = new WebAdmiral(KanColleClient.Current.Homeport.Admiral);
            admiral.AdmiralId = Admiral.AdmiralId;
            Context.Detach(Admiral);
            Context.AttachTo("Admirals", admiral);
            Context.ChangeState(admiral, EntityStates.Modified);
            try
            {
                Context.SaveChanges();
            }
            catch (DataServiceRequestException)
            {
                ResetContext();
                throw new DeniedAccessToAdmiral();
            }
        }

        void UpdateMasterData()
        {
            OutDebugConsole("UpdateMasterData");
            UpdateShipTypes();
            UpdateShipInfoes();
        }

        void UpdateShipTypes()
        {
            OutDebugConsole("UpdateShipTypes");
            var webShipTypes = Context.ShipTypes.Execute().ToList();
            foreach (var shipType in KanColleClient.Current.Master.ShipTypes)
            {
                try
                {
                    if (webShipTypes.Where(x =>
                        x.ShipTypeId == shipType.Value.Id &&
                        x.Name == shipType.Value.Name &&
                        x.SortNumber == shipType.Value.SortNumber
                        ).Count() == 0)
                    {
                        Context.AddToShipTypes(new WebShipType(shipType.Value));
                    }
                }
                catch (Exception ex)
                {
                    OutDebugConsole("Error: " + ex.ToString());
                }
            }
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                OutDebugConsole("Error: " + ex.ToString());
            }
        }

        void UpdateShipInfoes()
        {
            OutDebugConsole("UpdateShipInfoes");
            var webShipInfoes = Context.ShipInfoes.Execute().ToList();
            foreach (var shipInfo in KanColleClient.Current.Master.Ships)
            {
                if (shipInfo.Value.SortId != 0 && webShipInfoes.Where(x =>
                    x.ShipInfoId == shipInfo.Value.Id &&
                    x.Name == shipInfo.Value.Name &&
                    x.SortId == shipInfo.Value.SortId &&
                    x.NextRemodelingLevel == shipInfo.Value.NextRemodelingLevel
                    ).Count() == 0)
                {
                    Context.AddToShipInfoes(new WebShipInfo(shipInfo.Value));
                }
            }
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                OutDebugConsole("Error: " + ex.ToString());
            }
        }

        public async Task UpdateShips()
        {
            CheckAdmiral();
            await factory.StartNew(() =>
            {
                OutDebugConsole("UpdateShips");
                try
                {
                    var webShips = Context.Ships.Where(x => x.AdmiralId == Admiral.AdmiralId).ToList();
                    var ships = KanColleClient.Current.Homeport.Organization.Ships.ToList();
                    // まずは存在しない艦娘の削除と更新
                    foreach (var webShip in webShips)
                    {
                        if (ships.Where(x => x.Value.Id == webShip.ShipId).Count() == 0)
                        {
                            OutDebugConsole("Delete: " + webShip.ToString());
                            Context.DeleteObject(webShip);
                        }
                        else
                        {
                            var ship = new WebShip(ships.Where(x => x.Value.Id == webShip.ShipId).First().Value, Admiral.AdmiralId);
                            if (ship != webShip)
                            {
                                Context.Detach(webShip);
                                ship.ShipUid = webShip.ShipUid;
                                OutDebugConsole("Update: " + ship.ToString());
                                Context.AttachTo("Ships", ship);
                                Context.ChangeState(ship, EntityStates.Modified);
                            }
                        }
                    }
                    // 新しく手に入った艦娘の追加
                    foreach (var ship in ships)
                    {
                        if (webShips.Where(x => x.ShipId == ship.Value.Id).Count() == 0)
                        {
                            OutDebugConsole("Add: " + ship.Value.ToString());
                            Context.AddToShips(new WebShip(ship.Value, Admiral.AdmiralId));
                        }
                    }
                    OutDebugConsole("Saving ship data");
                    Context.SaveChanges();
                    OutDebugConsole("Saved ship data");
                }
                catch (DataServiceRequestException ex)
                {
                    ResetContext();
                    throw new DeniedAccessToAdmiral();
                }
            });
        }

        public async Task UpdateQuests(IEnumerable<Grabacr07.KanColleWrapper.Models.Raw.kcsapi_quest> quests)
        {
            CheckAdmiral();
            await factory.StartNew(() =>
            {
                OutDebugConsole("UpdateQuests");
                try
                {
                    var webQuests = Context.Quests.Where(x => x.AdmiralId == Admiral.AdmiralId).ToList();
                    // まずは存在しない任務の削除と更新
                    foreach (var webQuest in webQuests)
                    {
                        if (quests.Where(x => x.api_no == webQuest.QuestNo).Count() == 0)
                        {
                            OutDebugConsole("Delete: " + webQuest.ToString());
                            Context.DeleteObject(webQuest);
                        }
                        else
                        {
                            var quest = new WebQuest(quests.Where(x => x.api_no == webQuest.QuestNo).First(), Admiral.AdmiralId);
                            if (quest != webQuest)
                            {
                                Context.Detach(webQuest);
                                quest.QuestId = webQuest.QuestId;
                                OutDebugConsole("Update: " + quest.ToString());
                                Context.AttachTo("Quests", quest);
                                Context.UpdateObject(quest);
                            }
                        }
                    }
                    // 追加
                    foreach (var quest in quests)
                    {
                        if (webQuests.Where(x => x.QuestNo == quest.api_no).Count() == 0)
                        {
                            OutDebugConsole("Add: " + quest.ToString());
                            // 追加で何故か事故る
                            Context.AddToQuests(new WebQuest(quest, Admiral.AdmiralId));
                        }
                    }
                    OutDebugConsole("Saving quest data");
                    Context.SaveChanges();
                    OutDebugConsole("Saved quest data");
                    foreach (var quest in quests)
                    {
                        Context.Detach(quest);
                    }
                }
                catch (DataServiceRequestException ex)
                {
                    ResetContext();
                    throw new DeniedAccessToAdmiral();
                }
            });
        }
    }

    #region 例外クラス
    public class AdmiralNotInitialized : Exception
    {
        public AdmiralNotInitialized() { }
        public AdmiralNotInitialized(string message) : base(message) { }
    }

    public class DeniedAccessToAdmiral : Exception
    {
        public DeniedAccessToAdmiral() { }
        public DeniedAccessToAdmiral(string message) : base(message) { }
    }
    #endregion
}
