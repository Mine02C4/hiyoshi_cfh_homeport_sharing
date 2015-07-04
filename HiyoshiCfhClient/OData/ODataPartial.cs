using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiyoshiCfhClient.HiyoshiCfhWeb.Models
{
    public partial class Admiral
    {
        public Admiral() { }

        public Admiral(Grabacr07.KanColleWrapper.Models.Admiral admiral)
        {
            Name = admiral.Nickname;
            Experience = admiral.Experience;
            Level = admiral.Level;
            MemberId = int.Parse(admiral.MemberId);
            MaxShipCount = admiral.MaxShipCount;
            Rank = admiral.Rank;
        }
    }

    public partial class ShipType
    {
        public ShipType() { }

        public ShipType(Grabacr07.KanColleWrapper.Models.ShipType shipType)
        {
            ShipTypeId = shipType.Id;
            Name = shipType.Name;
            SortNumber = shipType.SortNumber;
        }

        public override string ToString()
        {
            return string.Format("ID = {0}, Name = \"{1}\"", this.ShipTypeId, this.Name);
        }
    }

    public partial class ShipInfo
    {
        public ShipInfo() { }

        public ShipInfo(Grabacr07.KanColleWrapper.Models.ShipInfo shipInfo)
        {
            ShipInfoId = shipInfo.Id;
            SortId = shipInfo.SortId;
            Name = shipInfo.Name;
            ShipTypeId = shipInfo.ShipType.Id;
            switch (shipInfo.Speed)
            {
                case Grabacr07.KanColleWrapper.Models.ShipSpeed.Fast:
                    ShipSpeed = HiyoshiCfhWeb.Models.ShipSpeed.Fast;
                    break;
                case Grabacr07.KanColleWrapper.Models.ShipSpeed.Low:
                    ShipSpeed = HiyoshiCfhWeb.Models.ShipSpeed.Low;
                    break;
            }
            NextRemodelingLevel = shipInfo.NextRemodelingLevel;
        }
    }

    public partial class Ship
    {
        public Ship() { }

        public Ship(Grabacr07.KanColleWrapper.Models.Ship ship)
        {
            ShipId = ship.Id;
            ShipInfoId = ship.Info.Id;
            Level = ship.Level;
            IsLocked = ship.IsLocked;
            Exp = ship.Exp;
            ExpForNextLevel = ship.ExpForNextLevel;
            Hp = ship.HP.Current;
            Fuel = ship.Fuel.Current;
            Bull = ship.Bull.Current;
            Firepower = ship.Firepower.Current;
            Torpedo = ship.Torpedo.Current;
            AA = ship.AA.Current;
            Armer = ship.Armer.Current;
            Luck = ship.Luck.Current;
        }

        public Ship(Grabacr07.KanColleWrapper.Models.Ship ship, int admiralId)
            : this(ship)
        {
            AdmiralId = admiralId;
        }

        public override string ToString()
        {
            return string.Format("ID = {0}, ShipInfoId = {1}, Level = {2}", this.ShipId, this.ShipInfoId, this.Level);
        }
    }
}
