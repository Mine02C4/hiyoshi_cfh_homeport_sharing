using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public class MaterialRecord
    {
        /// <summary>
        /// システム内部での識別子
        /// </summary>
        public int MaterialRecordId { get; set; }
        public int AdmiralId { get; set; }
        public virtual Admiral Admiral { get; set; }
        [IgnoreDataMember]
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime TimeUtc { get; set; }
        public MaterialType Type { get; set; }
        public int Value { get; set; }
    }

    public enum MaterialType
    {
        Fuel = 1,
        Bull = 2,
        Steel = 3,
        Bauxite = 4,
        InstantBuildMaterials = 5,
        InstantRepairMaterials = 6,
        DevelopmentMaterials = 7,
        RenovationMaterials = 8
    }
}