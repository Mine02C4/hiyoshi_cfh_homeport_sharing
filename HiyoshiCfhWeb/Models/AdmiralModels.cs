using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;

namespace HiyoshiCfhWeb.Models
{
    public class Admiral
    {
        /// <summary>
        /// システム内部での提督識別子。
        /// </summary>
        public int AdmiralId { get; set; }

        /// <summary>
        /// ゲーム内部での提督ID。
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 提督名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所有する艦娘。
        /// </summary>
        public virtual ICollection<Ship> Ships { get; set; }

        /// <summary>
        /// 提督経験値
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// 提督レベル
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 最大保有可能 艦娘数
        /// </summary>
        public int MaxShipCount { get; set; }

        /// <summary>
        /// 階級
        /// </summary>
        public string Rank { get; set; }

        [IgnoreDataMember]
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime UpdateUtc { get; set; }
    }
}
