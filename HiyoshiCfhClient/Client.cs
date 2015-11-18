using Grabacr07.KanColleWrapper;
using HiyoshiCfhClient.Default;
using WebAdmiral = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Admiral;
using WebShip = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Ship;
using WebShipType = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipType;
using WebShipInfo = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipInfo;
using WebSlotItem = HiyoshiCfhClient.HiyoshiCfhWeb.Models.SlotItem;
using WebSlotItemInfo = HiyoshiCfhClient.HiyoshiCfhWeb.Models.SlotItemInfo;
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
            OutDebugConsole("InitAdmiral TID = " + Thread.CurrentThread.ManagedThreadId.ToString());
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
            catch (DataServiceRequestException ex)
            {
                ResetContext();
                JudgeForbiddenOrNot(ex);
                throw ex;
            }
        }

        void UpdateMasterData()
        {
            OutDebugConsole("UpdateMasterData TID = " + Thread.CurrentThread.ManagedThreadId.ToString());
            UpdateShipTypes();
            UpdateShipInfoes();
            UpdateSlotItemInfoes();
        }

        void UpdateShipTypes()
        {
            OutDebugConsole("UpdateShipTypes");
            var webShipTypes = Context.ShipTypes.Execute().ToList();
            try
            {
                foreach (var shipType in KanColleClient.Current.Master.ShipTypes)
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
            try
            {
                SyncWithOData(KanColleClient.Current.Master.Ships.Values.Where(x => x.SortId != 0),
                    Context.ShipInfoes.Execute().ToList(), "ShipInfoes",
                    (x, y) => x.Id == y.ShipInfoId, x => new WebShipInfo(x),
                    x => Context.AddToShipInfoes(x));
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                OutDebugConsole("Error: " + ex.ToString());
            }
        }

        void UpdateSlotItemInfoes()
        {
            OutDebugConsole("UpdateSlotItemInfoes");
            try
            {
                SyncWithOData(KanColleClient.Current.Master.SlotItems.Values,
                    Context.SlotItemInfoes.Execute().ToList(), "SlotItemInfoes",
                    (x, y) => x.Id == y.SlotItemInfoId, x => new WebSlotItemInfo(x),
                    x => Context.AddToSlotItemInfoes(x));
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                OutDebugConsole("Error: " + ex.ToString());
            }
        }

        public async Task UpdateShips()
        {
            await factory.StartNew(() =>
            {
                OutDebugConsole("UpdateShips");
                CheckAdmiral();
                try
                {
                    SyncWithOData(KanColleClient.Current.Homeport.Organization.Ships.Values.ToList(),
                        Context.Ships.Where(x => x.AdmiralId == Admiral.AdmiralId).ToList(), "Ships",
                        (x, y) => x.Id == y.ShipId, x => new WebShip(x, Admiral.AdmiralId),
                        x => Context.AddToShips(x),
                        (x, y) =>
                        {
                            x.ShipUid = y.ShipUid;
                            return x;
                        }
                    );
                    OutDebugConsole("Saving ship data");
                    Context.SaveChanges();
                    OutDebugConsole("Saved ship data");
                }
                catch (DataServiceRequestException ex)
                {
                    ResetContext();
                    JudgeForbiddenOrNot(ex);
                    throw ex;
                }
            });
        }

        public async Task UpdateSlotItems()
        {
            await factory.StartNew(() =>
            {
                OutDebugConsole("UpdateSlotItems");
                CheckAdmiral();
                try
                {
                    SyncWithOData(KanColleClient.Current.Homeport.Itemyard.SlotItems.Values.ToList(),
                        Context.SlotItems.Where(x => x.AdmiralId == Admiral.AdmiralId).ToList(), "SlotItems",
                        (x, y) => x.Id == y.Id, x => new WebSlotItem(x, Admiral.AdmiralId),
                        x => Context.AddToSlotItems(x),
                        (x, y) =>
                        {
                            x.SlotItemUid = y.SlotItemUid;
                            return x;
                        }
                    );
                    OutDebugConsole("Saving SlotItem data");
                    Context.SaveChanges();
                    OutDebugConsole("Saved SlotItem data");
                }
                catch (DataServiceRequestException ex)
                {
                    ResetContext();
                    JudgeForbiddenOrNot(ex);
                    throw ex;
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
                    SyncWithOData(quests,
                        Context.Quests.Where(x => x.AdmiralId == Admiral.AdmiralId).ToList(),
                        "Quests", (x, y) => x.api_no == y.QuestNo,
                        x => new WebQuest(x, Admiral.AdmiralId),
                        x => Context.AddToQuests(x), (x, y) =>
                        {
                            x.QuestId = y.QuestId;
                            return x;
                        }
                    );
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
                    JudgeForbiddenOrNot(ex);
                    throw ex;
                }
            });
        }

        public async Task AddMaterialRecord(HiyoshiCfhWeb.Models.MaterialType type, int value)
        {
            await factory.StartNew(() =>
            {
                OutDebugConsole("AddMaterialRecord TID = " + Thread.CurrentThread.ManagedThreadId.ToString());
                CheckAdmiral();
                try
                {
                    var record = new HiyoshiCfhWeb.Models.MaterialRecord();
                    record.Type = type;
                    record.Value = value;
                    record.AdmiralId = Admiral.AdmiralId;
                    Context.AddToMaterialRecords(record);
                    Context.SaveChanges();
                }
                catch (DataServiceRequestException ex)
                {
                    ResetContext();
                    JudgeForbiddenOrNot(ex);
                    throw ex;
                }
            }
            );
        }

        void JudgeForbiddenOrNot(DataServiceRequestException ex)
        {
            if (ex.InnerException is DataServiceClientException)
            {
                var iex = ex.InnerException as DataServiceClientException;
                if (iex.StatusCode == 401)
                {
                    Admiral = null;
                    throw new DeniedAccessToAdmiral();
                }
            }
        }

        void SyncWithOData<T, U>(IEnumerable<T> localList, IEnumerable<U> odataList,
            string entitySetName,
            Func<T, U, bool> matching, Func<T, U> createOData, Action<U> addOData,
            Func<U, U, U> prepareOData = null, Func<U, bool> checkNull = null)
            where U : Microsoft.OData.Client.BaseEntityType
        {
            // まずは存在しないデータの削除と更新
            foreach (var odata in odataList)
            {
                if (localList.Where(x => matching(x, odata)).Count() == 0)
                {
                    OutDebugConsole("Delete: " + odata.ToString());
                    Context.DeleteObject(odata);
                }
                else
                {
                    var update = createOData(localList.Where(x => matching(x, odata)).First());
                    if (!update.Equals(odata) || (checkNull != null && checkNull(odata)))
                    {
                        Context.Detach(odata);
                        if (prepareOData != null)
                        {
                            update = prepareOData(update, odata);
                        }
                        OutDebugConsole("Update: " + update.ToString());
                        Context.AttachTo(entitySetName, update);
                        Context.ChangeState(update, EntityStates.Modified);
                    }
                }
            }
            // 新しいデータの追加
            foreach (var local in localList)
            {
                if (odataList.Where(x => matching(local, x)).Count() == 0)
                {
                    OutDebugConsole("Add: " + local.ToString());
                    addOData(createOData(local));
                }
            }
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
