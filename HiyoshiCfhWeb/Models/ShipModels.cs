using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;

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
        /// 艦娘のレベル。
        /// </summary>
        public int Level { get; set; }
        public bool IsLocked { get; set; }
        public int Exp { get; set; }
        public int ExpForNextLevel { get; set; }
        public int Hp { get; set; }
        public int Fuel { get; set; }
        public int Bull { get; set; }
        public int Firepower { get; set; }
        public int Torpedo { get; set; }
        public int AA { get; set; }
        public int Armer { get; set; }
        public int Luck { get; set; }
        // TODO: スロット情報は後回し。
        // public int[] Slots { get; set; }
    }

    public class ShipInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipInfoId { get; set; }
        public int SortId { get; set; }
        public string Name { get; set; }
        public int ShipTypeId { get; set; }
        public virtual ShipType ShipType { get; set; }
        public int[] Slots { get; set; }
        public ShipSpeed ShipSpeed { get; set; }
        public int? NextRemodelingLevel { get; set; }
    }

    public class ShipType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipTypeId { get; set; }
        public string Name { get; set; }
        public int SortNumber { get; set; }
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
