using Grabacr07.KanColleWrapper;
using HiyoshiCfhClient.Default;
using WebShipType = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipType;
using WebShipInfo = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;
using System.Collections.ObjectModel;

namespace HiyoshiCfhClient
{
    class Client
    {
        //Uri BaseUri;
        Container Context;
        string TokenType;
        string AccessToken;

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
            webShipInfo.Slots = new ObservableCollection<int>(shipInfo.Slots);
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

        public Client(string tokenType, string accessToken, string baseUri)
        {
            TokenType = tokenType;
            AccessToken = accessToken;
            //BaseUri = new Uri(baseUri);
            Context = new Container(new Uri("http://hiyoshicfhweb.azurewebsites.net/odata"));
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
