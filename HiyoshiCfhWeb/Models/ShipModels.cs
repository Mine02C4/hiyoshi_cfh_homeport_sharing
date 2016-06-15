using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HiyoshiCfhWeb.Models
{
    public class Ship
    {
        /// <summary>
        /// システム内部での識別子。各提督が保有する艦娘同士で一致することは無い。
        /// </summary>
        [Key]
        public int ShipUid { get; set; }

        /// <summary>
        /// 所有する提督。
        /// </summary>
        public int AdmiralId { get; set; }
        public virtual Admiral Admiral { get; set; }

        /// <summary>
        /// ゲーム内部で振られる各母港内での識別子。手に入れた順番に1から振られる。
        /// </summary>
        public int ShipId { get; set; }

        /// <summary>
        /// 艦娘マスタとの対応。
        /// </summary>
        public int ShipInfoId { get; set; }
        public virtual ShipInfo ShipInfo { get; set; }

        /// <summary>
        /// 艦娘のレベル。
        /// </summary>
        public int Level { get; set; }
        public bool IsLocked { get; set; }
        public int Exp { get; set; }
        public int ExpForNextLevel { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Fuel { get; set; }
        public int Bull { get; set; }
        public int Firepower { get; set; }
        public int Torpedo { get; set; }
        public int AA { get; set; }
        public int Armer { get; set; }
        public int Luck { get; set; }
        // TODO: スロット情報は後回し。
        // public int[] Slots { get; set; }
        /// <summary>
        /// 出撃識別札
        /// </summary>
        public int? SortieTag { get; set; }
        public virtual ICollection<SortieTagRecord> SortieTagRecords { get; set; }

        [NotMapped]
        public int? LevelForNextRemodeling
        {
            get
            {
                if (ShipInfo.NextRemodelingLevel == null)
                {
                    return null;
                }
                else
                {
                    return ShipInfo.NextRemodelingLevel - Level;
                }
            }
        }
    }

    public class SortieTagRecord
    {
        [Key]
        [Column(Order = 0)]
        public int ShipUid { get; set; }
        [Key]
        [Column(Order = 1)]
        public int EventId { get; set; }
        public int SortieTagId { get; set; }
        public virtual Ship Ship { get; set; }

        [NotMapped]
        public Event Event
        {
            get
            {
                return Event.Events.Where(x => x.Id == EventId).First();
            }
        }

        [NotMapped]
        public SortieTag SortieTag
        {
            get
            {
                return Event.SortieTags.Where(x => x.InternalId == SortieTagId).First();
            }
        }
    }

    public class ShipInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipInfoId { get; set; }
        public int SortId { get; set; }
        public string Name { get; set; }
        public int ShipTypeId { get; set; }
        public virtual ShipType ShipType { get; set; }
        public ShipSpeed ShipSpeed { get; set; }
        public int? NextRemodelingLevel { get; set; }
        public string Kana { get; set; }
        /// <summary>
        /// 耐久の最大値。ケッコンカッコカリではこの値ではなくなる。
        /// </summary>
        public int MaxHp { get; set; }
        public int MaxFuel { get; set; }
        public int MaxBull { get; set; }
        public int MaxFirepower { get; set; }
        public int MaxTorpedo { get; set; }
        public int MaxAA { get; set; }
        public int MaxArmer { get; set; }
        public int MaxLuck { get; set; }
        public int MinLuck { get; set; }
    }

    public class ShipType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipTypeId { get; set; }
        public string Name { get; set; }
        public int SortNumber { get; set; }

        public static string GroupShipType(string type)
        {
            switch (type)
            {
                case "戦艦":
                case "航空戦艦":
                    type = "戦艦+航空戦艦";
                    break;
                case "潜水艦":
                case "潜水空母":
                    type = "潜水艦+潜水空母";
                    break;
                case "正規空母":
                case "装甲空母":
                    type = "正規空母+装甲空母";
                    break;
                default:
                    break;
            }
            return type;
        }
    }

    public enum ShipSpeed
    {
        /// <summary>
        /// 低速艦
        /// </summary>
        Low = 10,

        /// <summary>
        /// 高速艦
        /// </summary>
        Fast = 20,
    }
}
