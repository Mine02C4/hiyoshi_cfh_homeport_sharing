using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public class Quest
    {
        /// <summary>
        /// システム内部での識別子
        /// </summary>
        public int QuestId { get; set; }

        /// <summary>
        /// ゲーム内での任務番号(api_no)
        /// </summary>
        public int QuestNo { get; set; }

        public int AdmiralId { get; set; }
        public virtual Admiral Admiral { get; set; }

        public QuestCategory Category { get; set; }

        public QuestType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        public int Fuel { get; set; }

        public int Bull { get; set; }

        public int Steel { get; set; }

        public int Bauxite { get; set; }
    }

    public enum QuestCategory
    {
        /// <summary>
        /// 編成
        /// </summary>
        Composition = 1,

        /// <summary>
        /// 出撃
        /// </summary>
        Sortie = 2,

        /// <summary>
        /// 演習
        /// </summary>
        Practice = 3,

        /// <summary>
        /// 遠征
        /// </summary>
        Expeditions = 4,

        /// <summary>
        /// 補給/入渠
        /// </summary>
        Supply = 5,

        /// <summary>
        /// 工廠
        /// </summary>
        Building = 6,

        /// <summary>
        /// 改装
        /// </summary>
        Remodelling = 7
    }

    public enum QuestType
    {
        /// <summary>
        /// 1回
        /// </summary>
        OneTime = 1,

        /// <summary>
        /// デイリー
        /// </summary>
        Daily = 2,

        /// <summary>
        /// ウィークリー
        /// </summary>
        Weekly = 3,

        /// <summary>
        /// マンスリー
        /// </summary>
        Monthly = 4
    }
}