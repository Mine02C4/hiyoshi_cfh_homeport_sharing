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

    public static class Material {
        public static List<MaterialTuple> List;

        static Material()
        {
            List = new List<MaterialTuple>();
            List.Add(new MaterialTuple("燃料", MaterialType.Fuel));
            List.Add(new MaterialTuple("弾薬", MaterialType.Bull));
            List.Add(new MaterialTuple("鋼材", MaterialType.Steel));
            List.Add(new MaterialTuple("ボーキサイト", MaterialType.Bauxite));
            List.Add(new MaterialTuple("高速建造材", MaterialType.InstantBuildMaterials));
            List.Add(new MaterialTuple("高速修復材", MaterialType.InstantRepairMaterials));
            List.Add(new MaterialTuple("開発資材", MaterialType.DevelopmentMaterials));
            List.Add(new MaterialTuple("改修資材", MaterialType.RenovationMaterials));
        }
    }

    public class MaterialTuple
    {
        public string Name { get; private set; }
        public MaterialType Type { get; private set; }
        public MaterialTuple(string name, MaterialType type)
        {
            Name = name;
            Type = type;
        }
    }
}