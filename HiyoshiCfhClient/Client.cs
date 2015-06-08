using Grabacr07.KanColleWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiyoshiCfhClient
{
    class Client
    {
        private Uri BaseUri;
        public Client(string baseUri)
        {
            //BaseUri = new Uri(baseUri);
        }

        public string CollectShipsData()
        {
            var ships = KanColleClient.Current.Homeport.Organization.Ships;
            var shipTypes = KanColleClient.Current.Master.Ships
                .GroupBy(x => x.Value.ShipType, (key, elements) => key);
            var shipTypes2 = KanColleClient.Current.Master.ShipTypes; // こっち採用
            string output = "shipTypes" + System.Environment.NewLine;
            foreach (var shipType in shipTypes)
            {
                output += shipType.ToString() + System.Environment.NewLine;
            }
            output += "shipTypes2" + System.Environment.NewLine;
            foreach (var shipType in shipTypes2)
            {
                output += shipType.ToString() + System.Environment.NewLine;
            }
            output += "ships" + System.Environment.NewLine;
            foreach (var ship in ships)
            {
                output += ship.ToString() + System.Environment.NewLine;
            }
            return output;
        }
    }
}
