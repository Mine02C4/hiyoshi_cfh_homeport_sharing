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

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var a = (ShipType)obj;
            return this == a;
        }

        public override int GetHashCode()
        {
            return ShipTypeId ^ SortNumber;
        }

        public static bool operator !=(ShipType a, ShipType b)
        {
            return !(a == b);
        }

        public static bool operator ==(ShipType a, ShipType b)
        {
            return Util.EqualModel<ShipType>(a, b);
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

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var a = (ShipInfo)obj;
            return this == a;
        }

        public override int GetHashCode()
        {
            return ShipInfoId ^ SortId;
        }

        public static bool operator !=(ShipInfo a, ShipInfo b)
        {
            return !(a == b);
        }

        public static bool operator ==(ShipInfo a, ShipInfo b)
        {
            return Util.EqualModel<ShipInfo>(a, b);
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
            SortieTag = ship.SallyArea;
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

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var a = (Ship)obj;
            return this == a;
        }

        public override int GetHashCode()
        {
            return ShipId ^ Hp ^ Fuel ^ Bull;
        }

        public static bool operator !=(Ship a, Ship b)
        {
            return !(a == b);
        }

        public static bool operator ==(Ship a, Ship b)
        {
            return Util.EqualModel<Ship>(a, b, new string[] { "ShipUid" });
        }
    }

    public partial class Quest
    {
        public Quest() { }

        public Quest(Grabacr07.KanColleWrapper.Models.Raw.kcsapi_quest quest, int admiralId)
        {
            QuestNo = quest.api_no;
            Category = (QuestCategory)quest.api_category;
            switch (quest.api_type)
            {
                case 1:
                    Type = QuestType.OneTime;
                    break;
                case 2:
                case 4:
                case 5:
                    Type = QuestType.Daily;
                    break;
                case 3:
                    Type = QuestType.Weekly;
                    break;
                default:
                    Type = QuestType.Monthly;
                    break;
            }
            Name = quest.api_title;
            Content = quest.api_detail;
            Fuel = quest.api_get_material[0];
            Bull = quest.api_get_material[1];
            Steel = quest.api_get_material[2];
            Bauxite = quest.api_get_material[3];
            AdmiralId = admiralId;
        }

        public override string ToString()
        {
            return string.Format("ID = {0}, No = {1}, Name = {2}", this.QuestId, this.QuestNo, this.Name);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var a = (Quest)obj;
            return this == a;
        }

        public override int GetHashCode()
        {
            return QuestId ^ QuestNo ^ Fuel ^ Bull ^ Steel ^ Bauxite;
        }

        public static bool operator !=(Quest a, Quest b)
        {
            return !(a == b);
        }

        public static bool operator ==(Quest a, Quest b)
        {
            return Util.EqualModel<Quest>(a, b, new string[] { "QuestId" });
        }
    }

    static class Util
    {
        public static bool EqualModel<T>(T a, T b, string[] ignoreProperties = null)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                if ((ignoreProperties == null || !(ignoreProperties.Any(key => property.Name == key))) && type.IsValueType
                    && property.GetValue(a) != null && property.GetValue(b) != null
                    && !Convert.ChangeType(property.GetValue(a), type).Equals(Convert.ChangeType(property.GetValue(b), type)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
