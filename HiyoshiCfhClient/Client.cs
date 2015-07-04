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
            var admiral = new WebAdmiral(KanColleClient.Current.Homeport.Admiral);
            Context.AddToAdmirals(admiral);
            await Context.SaveChangesAsync();
            Admiral = admiral;
        }

        public async Task UpdateAdmiral()
        {
            OutDebugConsole("UpdateAdmiral");
            var admiral = new WebAdmiral(KanColleClient.Current.Homeport.Admiral);
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
                        Context.AddToShipTypes(new WebShipType(shipType.Value));
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
                    Context.AddToShipInfoes(new WebShipInfo(shipInfo.Value));
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
    }
}
