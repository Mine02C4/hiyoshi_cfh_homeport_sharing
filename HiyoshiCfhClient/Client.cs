﻿using Grabacr07.KanColleWrapper;
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
            await Context.SaveChangesAsync();
            output += "ships" + System.Environment.NewLine;
            foreach (var ship in ships)
            {
                output += ship.ToString() + System.Environment.NewLine;
            }
            return output;
        }

        public async Task<string> GetShipTypes()
        {
            Context.SendingRequest2 += (sender, eventArgs) =>
            {
                eventArgs.RequestMessage.SetHeader("Authorization",  TokenType + " " + AccessToken);
            };
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
                return ex.Message;
            }
        }
    }
}
