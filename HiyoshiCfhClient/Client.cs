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

namespace HiyoshiCfhClient
{
    class Client
    {
        Container Context;
        string TokenType;
        string AccessToken;
        WebAdmiral Admiral;
        public delegate void DebugConsole(string msg);
        DebugConsole _DebugConsole;

        private static WebShipType ConvertShipType(ShipType shipType)
        {
            var webShipType = new WebShipType();
            webShipType.ShipTypeId = shipType.Id;
            webShipType.Name = shipType.Name;
            webShipType.SortNumber = shipType.SortNumber;
            return webShipType;
        }

        private static WebShipInfo ConvertShipInfo(ShipInfo shipInfo)
        {
            var webShipInfo = new WebShipInfo();
            webShipInfo.ShipInfoId = shipInfo.Id;
            webShipInfo.SortId = shipInfo.SortId;
            webShipInfo.Name = shipInfo.Name;
            webShipInfo.ShipTypeId = shipInfo.ShipType.Id;
            switch (shipInfo.Speed)
            {
                case ShipSpeed.Fast:
                    webShipInfo.ShipSpeed = HiyoshiCfhWeb.Models.ShipSpeed.Fast;
                    break;
                case ShipSpeed.Low:
                    webShipInfo.ShipSpeed = HiyoshiCfhWeb.Models.ShipSpeed.Low;
                    break;
            }
            webShipInfo.NextRemodelingLevel = shipInfo.NextRemodelingLevel;
            return webShipInfo;
        }

        private static WebAdmiral ConvertAdmiral(Admiral admiral)
        {
            var webAdmiral = new WebAdmiral();
            webAdmiral.Name = admiral.Nickname;
            webAdmiral.Experience = admiral.Experience;
            webAdmiral.Level = admiral.Level;
            webAdmiral.MemberId = int.Parse(admiral.MemberId);
            webAdmiral.MaxShipCount = admiral.MaxShipCount;
            webAdmiral.Rank = admiral.Rank;
            return webAdmiral;
        }

        private static WebShip ConvertShip(Ship ship, int admiralId)
        {
            var webShip = new WebShip();
            webShip.AdmiralId = admiralId;
            webShip.ShipId = ship.Id;
            webShip.ShipInfoId = ship.Info.Id;
            webShip.Level = ship.Level;
            webShip.IsLocked = ship.IsLocked;
            webShip.Exp = ship.Exp;
            webShip.ExpForNextLevel = ship.ExpForNextLevel;
            webShip.Hp = ship.HP.Current;
            webShip.Fuel = ship.Fuel.Current;
            webShip.Bull = ship.Bull.Current;
            webShip.Firepower = ship.Firepower.Current;
            webShip.Torpedo = ship.Torpedo.Current;
            webShip.AA = ship.AA.Current;
            webShip.Armer = ship.Armer.Current;
            webShip.Luck = ship.Luck.Current;
            return webShip;
        }

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

        public async Task SendShipTypes()
        {
            await Task.Run(() =>
            {
                var shipTypes = KanColleClient.Current.Master.ShipTypes;
                foreach (var shipType in shipTypes)
                {
                    var webShipType = ConvertShipType(shipType.Value);
                    Context.AddToShipTypes(webShipType);
                }
                Context.SaveChanges();
            });
        }

        public async Task SendShipInfoes()
        {
            await Task.Run(() =>
            {
                var shipInfoes = KanColleClient.Current.Master.Ships;
                foreach (var shipInfo in shipInfoes)
                {
                    if (shipInfo.Value.SortId != 0)
                    {
                        var webShipInfo = ConvertShipInfo(shipInfo.Value);
                        Context.AddToShipInfoes(webShipInfo);
                    }
                }
                Context.SaveChanges();
            });
        }

        public async Task InitAdmiralInformation()
        {
            OutDebugConsole("InitAdmiralInformation");
            var memberId = int.Parse(KanColleClient.Current.Homeport.Admiral.MemberId);
            Admiral = await GetAdmiral(memberId);
            if (Admiral == null)
            {
                await RegisterAdmiral();
                Admiral = await GetAdmiral(memberId);
            }
            else
            {
                await UpdateAdmiral();
            }
        }

        async Task<WebAdmiral> GetAdmiral(int MemberId)
        {
            OutDebugConsole("GetAdmiral");
            return await Task.Run(() =>
            {
                return Context.Admirals.Where(x => x.MemberId == MemberId).FirstOrDefault();
            });
        }

        public async Task RegisterAdmiral()
        {
            OutDebugConsole("RegisterAdmiral");
            // TODO: 登録エラーの実装
            var admiral = ConvertAdmiral(KanColleClient.Current.Homeport.Admiral);
            Context.AddToAdmirals(admiral);
            await Context.SaveChangesAsync();
            Admiral = admiral;
        }

        public async Task UpdateAdmiral()
        {
            OutDebugConsole("UpdateAdmiral");
            var admiral = ConvertAdmiral(KanColleClient.Current.Homeport.Admiral);
            admiral.AdmiralId = Admiral.AdmiralId;
            Context.Detach(Admiral);
            Context.AttachTo("Admirals", admiral);
            Context.ChangeState(admiral, EntityStates.Modified);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateMasterData()
        {
            OutDebugConsole("UpdateMasterData");
            await UpdateShipTypes();
            await UpdateShipInfoes();
        }

        async Task UpdateShipTypes()
        {
            OutDebugConsole("UpdateShipTypes");
            var webShipTypes = (await GetShipTypesFromServer()).ToList();
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
                        Context.AddToShipTypes(ConvertShipType(shipType.Value));
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error: {0}", ex);
                }
            }
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: {0}", ex);
            }
        }

        async Task UpdateShipInfoes()
        {
            OutDebugConsole("UpdateShipInfoes");
            var webShipInfoes = (await GetShipInfoesFromServer()).ToList();
            foreach (var shipInfo in KanColleClient.Current.Master.Ships)
            {
                if (shipInfo.Value.SortId != 0 && webShipInfoes.Where(x =>
                    x.ShipInfoId == shipInfo.Value.Id &&
                    x.Name == shipInfo.Value.Name &&
                    x.SortId == shipInfo.Value.SortId &&
                    x.NextRemodelingLevel == shipInfo.Value.NextRemodelingLevel
                    ).Count() == 0)
                {
                    Context.AddToShipInfoes(ConvertShipInfo(shipInfo.Value));
                }
            }
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: {0}", ex);
            }
        }

        async Task<IEnumerable<WebShipType>> GetShipTypesFromServer()
        {
            return await Context.ShipTypes.ExecuteAsync();
        }

        async Task<IEnumerable<WebShipInfo>> GetShipInfoesFromServer()
        {
            return await Context.ShipInfoes.ExecuteAsync();
        }

        public async Task UpdateShips()
        {
            OutDebugConsole("UpdateShips");
            var webShips = GetShipFromServer().ToList();
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
                    Context.Detach(webShip);
                    var ship = ConvertShip(ships.Where(x => x.Value.Id == webShip.ShipId).First().Value, Admiral.AdmiralId);
                    ship.ShipUid = webShip.ShipUid;
                    OutDebugConsole("Update: " + ship.ToString());
                    Context.AttachTo("Ships", ship);
                    Context.ChangeState(ship, EntityStates.Modified);
                }
            }
            // 新しく手に入った艦娘の追加
            foreach (var ship in ships)
            {
                if (webShips.Where(x => x.ShipId == ship.Value.Id).Count() == 0)
                {
                    OutDebugConsole("Add: " + ship.Value.ToString());
                    Context.AddToShips(ConvertShip(ship.Value, Admiral.AdmiralId));
                }
            }
            try
            {
                OutDebugConsole("Saving ship data");
                await Context.SaveChangesAsync();
                OutDebugConsole("Saved ship data");
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: {0}", ex);
                OutDebugConsole("Error: " + ex.ToString());
            }
        }

        public IQueryable<WebShip> GetShipFromServer()
        {
            return Context.Ships.Where(x => x.AdmiralId == Admiral.AdmiralId);
        }

        public async Task<string> CollectShipsData()
        {
            var ships = KanColleClient.Current.Homeport.Organization.Ships;
            var shipTypes = KanColleClient.Current.Master.ShipTypes;
            string output = "shipTypes" + System.Environment.NewLine;
            foreach (var shipType in shipTypes)
            {
                output += shipType.ToString() + System.Environment.NewLine;
                var webShipType = ConvertShipType(shipType.Value);
                Context.AddToShipTypes(webShipType);
            }
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ex.Message + System.Environment.NewLine;
            }
            output += "ships" + System.Environment.NewLine;
            foreach (var ship in ships)
            {
                output += ship.ToString() + System.Environment.NewLine;
            }
            return output;
        }

        public async Task<string> GetShipTypes()
        {
            try
            {
                var shipTypes = await Context.ShipTypes.ExecuteAsync();
                string output = "shipTypes(Server)" + System.Environment.NewLine;
                foreach (var shipType in shipTypes)
                {
                    output += shipType.ToString() + System.Environment.NewLine;
                }
                return output;
            }
            catch (Exception ex)
            {
                return ex.Message + System.Environment.NewLine;
            }
        }
    }
}
