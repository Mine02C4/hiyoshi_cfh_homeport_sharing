using Grabacr07.KanColleWrapper;
using HiyoshiCfhClient.Default;
using WebShipType = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipType;
using WebShipInfo = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipInfo;
using WebAdmiral = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Admiral;
using WebShip = HiyoshiCfhClient.HiyoshiCfhWeb.Models.Ship;
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
            Context = new Container(new Uri("http://hiyoshicfhweb.azurewebsites.net/odata"));
            if (tokenType != null && accessToken != null)
            {
                Context.SendingRequest2 += (sender, eventArgs) =>
                {
                    eventArgs.RequestMessage.SetHeader("Authorization", TokenType + " " + AccessToken);
                };
            }
            taskScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            factory = new TaskFactory(taskScheduler);
        }

        public Client(string tokenType, string accessToken, DebugConsole debugConsole)
            : this(tokenType, accessToken)
        {
            _DebugConsole = debugConsole;
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
                catch (DataServiceRequestException)
                {
                    throw new DeniedAccessToAdmiral();
                }
            });
        }

        /// <summary>
        /// 使用するスレッドの上限数を設けたタスクスケジューラー
        /// </summary>
        class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
        {
            /// <summary>
            /// 現在のスレッドがタスクを実行しているかどうか
            /// </summary>
            [ThreadStatic]
            private static bool _currentThreadIsProcessingItems;

            /// <summary>
            /// 実行されるタスクのリスト
            /// </summary>
            private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)

            /// <summary>
            /// 同時実行可能なタスク数の上限
            /// </summary>
            private readonly int _maxDegreeOfParallelism;

            /// <summary>
            /// 現在実行しているタスク数
            /// </summary>
            private int _delegatesQueuedOrRunning = 0;

            /// <summary>
            /// 上限数を指定してインスタンスを生成
            /// </summary>
            /// <param name="maxDegreeOfParallelism">同時実行可能なタスクの上限数(1以上の整数)</param>
            public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
            {
                if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
                _maxDegreeOfParallelism = maxDegreeOfParallelism;
            }

            /// <summary>
            /// タスクをタスクキューに追加
            /// </summary>
            /// <param name="task">追加するタスク</param>
            protected sealed override void QueueTask(Task task)
            {
                lock (_tasks)
                {
                    _tasks.AddLast(task);
                    if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                    {
                        _delegatesQueuedOrRunning++;
                        NotifyThreadPoolOfPendingWork();
                    }
                }
            }

            /// <summary>
            /// 実行すべき作業があることをThreadPoolに通知します
            /// </summary>
            private void NotifyThreadPoolOfPendingWork()
            {
                ThreadPool.UnsafeQueueUserWorkItem(_ =>
                {
                    _currentThreadIsProcessingItems = true;
                    try
                    {
                        while (true)
                        {
                            Task item;
                            lock (_tasks)
                            {
                                if (_tasks.Count == 0)
                                {
                                    _delegatesQueuedOrRunning--;
                                    break;
                                }
                                item = _tasks.First.Value;
                                _tasks.RemoveFirst();
                            }
                            base.TryExecuteTask(item);
                        }
                    }
                    finally { _currentThreadIsProcessingItems = false; }
                }, null);
            }

            /// <summary>
            /// 現在のスレッドでタスクを実行しようとします
            /// </summary>
            /// <param name="task"></param>
            /// <param name="taskWasPreviouslyQueued"></param>
            /// <returns></returns>
            protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                if (!_currentThreadIsProcessingItems) return false;
                if (taskWasPreviouslyQueued)
                {
                    if (TryDequeue(task))
                        return base.TryExecuteTask(task);
                    else
                        return false;
                }
                else
                {
                    return base.TryExecuteTask(task);
                }
            }

            /// <summary>
            /// 事前にスケジュールされてたタスクをスケジューラーから取り除きます
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            protected sealed override bool TryDequeue(Task task)
            {
                lock (_tasks) return _tasks.Remove(task);
            }

            public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

            protected sealed override IEnumerable<Task> GetScheduledTasks()
            {
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(_tasks, ref lockTaken);
                    if (lockTaken) return _tasks;
                    else throw new NotSupportedException();
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(_tasks);
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
