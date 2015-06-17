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
    }

    public class SlotItemInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SlotItemInfoId { get; set; }
    }
}