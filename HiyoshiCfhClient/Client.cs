using Grabacr07.KanColleWrapper;
using HiyoshiCfhClient.Default;
using WebShipType = HiyoshiCfhClient.HiyoshiCfhWeb.Models.ShipType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;

namespace HiyoshiCfhClient
{
    class Client
    {
        private Uri BaseUri;
        Container context;

        private static WebShipType ConvertShipType(ShipType shipType)
        {
            var webShipType = new WebShipType();
            webShipType.ShipTypeId = shipType.Id;
            webShipType.Name = shipType.Name;
            webShipType.SortNumber = shipType.SortNumber;
            return webShipType;
        }

        public Client(string baseUri)
        {
            //BaseUri = new Uri(baseUri);
            context = new Container(new Uri("http://hiyoshicfhweb.azurewebsites.net/odata"));
        }

        public async Task<string> CollectShipsData()
        {
            var ships = KanColleClient.Current.Homeport.Organization.Ships;
            var shipTypes2 = KanColleClient.Current.Master.ShipTypes;
            string output = "shipTypes" + System.Environment.NewLine;
            foreach (var shipType in shipTypes2)
            {
                output += shipType.ToString() + System.Environment.NewLine;
                var webShipType = ConvertShipType(shipType.Value);
                context.AddToShipTypes(webShipType);
            }
            await context.SaveChangesAsync();
            output += "ships" + System.Environment.NewLine;
            foreach (var ship in ships)
            {
                output += ship.ToString() + System.Environment.NewLine;
            }
            return output;
        }
    }
}
