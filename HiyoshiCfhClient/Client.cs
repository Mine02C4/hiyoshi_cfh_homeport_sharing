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

        public Client()
        {
            context = new Container(new Uri("http://hiyoshicfhweb.azurewebsites.net/odata"));
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

        public async Task<string> GetShipTypes()
        {
            context.SendingRequest2 += (sender, eventArgs) =>
            {
                eventArgs.RequestMessage.SetHeader("Authorization", "bearer 2NwadvQfvCnKs0C-pUGyMZK6DLU5211LyT6xh2Vssud5Og1qr1YZa0AQFYCEWRfjdQx9dTPQ-1cyNTBf4REcxpFKN4SRoUO1qsmIL57KQiodaw-JLgZZaBxEw3Ee-hOUDU3t8rwFWw6cPhH36HmzqPdbM_PZcs8KeTKPfR9-B6kKTHMNmQWTwndBoIKOoKFBJLcXF1uZ8u7I2XpEcCHkPfYZKhXwbdkcDlR8YENxVWhe_SixceXg9Qojl7O_T2AJtZSXJeNyKVyT9-2Kfl3peoBRUimCrB7s10SEetRKJJXzYvgkR_3Pa232Mgs6-nyoHmW9xO3I66pV4bqDSWWX0qIupQm9YQnuMBy3_iit6Leonk471HePGIdlF6LJ3r4IQjs2QRPmuS-0wdMFfy82oTgY_fbUjlmlXyAUaHLKgJ99j73YscEyBsBuqzikYNdubs_08J3Z5olYlafrQOb2fnkoyqATJvxL5xQZi7shPWNJ2XGicKC5THI9zWtmMHSD");
            };
            try
            {
                var shipTypes = await context.ShipTypes.ExecuteAsync();
                string output = "shipTypes(Server)" + System.Environment.NewLine;
                foreach (var shipType in shipTypes)
                {
                    output += shipType.ToString() + System.Environment.NewLine;
                }
                return output;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
