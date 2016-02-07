using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public class SlotItem
    {
        /// <summary>
        /// システム内部での識別子。各提督が保有する装備同士で一致することは無い。
        /// </summary>
        [Key]
        public int SlotItemUid { get; set; }

        /// <summary>
        /// 所有する提督。
        /// </summary>
        public int AdmiralId { get; set; }
        public virtual Admiral Admiral { get; set; }

        /// <summary>
        /// ゲーム内部で振られる各母港内での識別子。
        /// </summary>
        public int Id { get; set; }

        public int SlotItemInfoId { get; set; }
        public virtual SlotItemInfo SlotItemInfo { get; set; }

        /// <summary>
        /// 改修レベル
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 熟練度
        /// </summary>
        public int Adept { get; set; }
    }

    public class SlotItemInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SlotItemInfoId { get; set; }
        public string Name { get; set; }
        public SlotItemType Type { get; set; }
        public int CategoryId { get; set; }
        public int Firepower { get; set; }
        public int Torpedo { get; set; }
        public int AA { get; set; }
        public int Armer { get; set; }
        public int Bomb { get; set; }
        public int AS { get; set; }
        public int Hit { get; set; }
        public int Evasiveness { get; set; }
        public int Search { get; set; }

        [NotMapped]
        public bool IsAircraft
        {
            get
            {
                return (CategoryId >= 6 && CategoryId <= 10) || CategoryId == 33;
            }
        }
    }

    public enum SlotItemType
    {
        不明 = 0,
        艦上戦闘機 = 6,
        艦上爆撃機 = 7,
        艦上攻撃機 = 8,
        艦上偵察機 = 9,
        水上偵察機 = 10,
        水上爆撃機 = 11,
        小型電探 = 12,
        大型電探 = 13,
        応急修理要員 = 23,
        探照灯 = 29,
    }
}
