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
            NextRemodelingLevel = shipInfo.NextRemodelingLevel;
            Kana = shipInfo.Kana;
            MaxHp = shipInfo.HP;
            MaxFuel = shipInfo.MaxFuel;
            MaxBull = shipInfo.MaxBull;
            MaxFirepower = shipInfo.MaxFirepower;
            MaxTorpedo = shipInfo.MaxTorpedo;
            MaxAA = shipInfo.MaxAA;
            MaxArmer = shipInfo.MaxArmer;
            MaxLuck = shipInfo.MaxLuck;
            MinLuck = shipInfo.MinLuck;
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
            MaxHp = ship.HP.Maximum;
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

    public partial class SlotItem
    {
        public SlotItem() { }

        public SlotItem(Grabacr07.KanColleWrapper.Models.SlotItem slotItem)
        {
            Id = slotItem.Id;
            SlotItemInfoId = slotItem.Info.Id;
            Level = slotItem.Level;
            Adept = slotItem.Proficiency;
        }

        public SlotItem(Grabacr07.KanColleWrapper.Models.SlotItem slotItem, int admiralId)
            : this(slotItem)
        {
            AdmiralId = admiralId;
        }

        public override string ToString()
        {
            return string.Format("ID = {0}, SlotItemInfoId = {1}", this.Id, this.SlotItemInfoId);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var a = (SlotItem)obj;
            return this == a;
        }

        public override int GetHashCode()
        {
            return Id ^ Level ^ Adept;
        }

        public static bool operator !=(SlotItem a, SlotItem b)
        {
            return !(a == b);
        }

        public static bool operator ==(SlotItem a, SlotItem b)
        {
            return Util.EqualModel<SlotItem>(a, b, new string[] { "SlotItemUid" });
        }
    }

    public partial class SlotItemInfo
    {
        public SlotItemInfo() { }

        public SlotItemInfo(Grabacr07.KanColleWrapper.Models.SlotItemInfo slotItemInfo)
        {
            SlotItemInfoId = slotItemInfo.Id;
            Name = slotItemInfo.Name;
            switch (slotItemInfo.Type)
            {
                case Grabacr07.KanColleWrapper.Models.SlotItemType.大型電探:
                    Type = SlotItemType.大型電探;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.小型電探:
                    Type = SlotItemType.小型電探;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.応急修理要員:
                    Type = SlotItemType.応急修理要員;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.探照灯:
                    Type = SlotItemType.探照灯;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.水上偵察機:
                    Type = SlotItemType.水上偵察機;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.水上爆撃機:
                    Type = SlotItemType.水上爆撃機;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.艦上偵察機:
                    Type = SlotItemType.艦上偵察機;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.艦上戦闘機:
                    Type = SlotItemType.艦上戦闘機;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.艦上攻撃機:
                    Type = SlotItemType.艦上攻撃機;
                    break;
                case Grabacr07.KanColleWrapper.Models.SlotItemType.艦上爆撃機:
                    Type = SlotItemType.艦上爆撃機;
                    break;
                default:
                    Type = SlotItemType.不明;
                    break;
            }
            CategoryId = slotItemInfo.CategoryId;
            Firepower = slotItemInfo.Firepower;
            Torpedo = slotItemInfo.Torpedo;
            AA = slotItemInfo.AA;
            Armer = slotItemInfo.Armer;
            Bomb = slotItemInfo.Bomb;
            AS = slotItemInfo.ASW;
            Hit = slotItemInfo.Hit;
            Evasiveness = slotItemInfo.Evade;
            Search = slotItemInfo.ViewRange;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var a = (SlotItemInfo)obj;
            return this == a;
        }

        public override int GetHashCode()
        {
            return SlotItemInfoId ^ CategoryId;
        }

        public static bool operator !=(SlotItemInfo a, SlotItemInfo b)
        {
            return !(a == b);
        }

        public static bool operator ==(SlotItemInfo a, SlotItemInfo b)
        {
            return Util.EqualModel<SlotItemInfo>(a, b);
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
                    Type = QuestType.Daily;
                    break;
                case 2:
                    Type = QuestType.Weekly;
                    break;
                case 3:
                    Type = QuestType.Monthly;
                    break;
                case 4:
                    Type = QuestType.OneTime;
                    break;
                case 5:
                    Type = QuestType.Other;
                    break;
                default:
                    Type = QuestType.Other;
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
                if (ignoreProperties == null || !(ignoreProperties.Any(key => property.Name == key)))
                {
                    var pa = property.GetValue(a);
                    var pb = property.GetValue(b);
                    if (type.IsValueType || type == typeof(string))
                    {
                        if (pa == null || pb == null)
                        {
                            if (pa != pb)
                            {
                                return false;
                            }
                        }
                        else if (!Convert.ChangeType(pa, type).Equals(Convert.ChangeType(pb, type)))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
