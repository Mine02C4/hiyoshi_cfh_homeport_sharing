﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// このソース コードは xsd によって自動生成されました。Version=4.6.1055.0 です。
// 
namespace HiyoshiCfhWeb.XML {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd", IsNullable=false)]
    public partial class Quests {
        
        private Quest[] questField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Quest")]
        public Quest[] Quest {
            get {
                return this.questField;
            }
            set {
                this.questField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd")]
    public partial class Quest {
        
        private string nameField;
        
        private string contentField;
        
        private Bonus bonusField;
        
        private DependencyAchieve[] dependencyField;
        
        private string idField;
        
        private Category categoryField;
        
        private bool categoryFieldSpecified;
        
        private Type typeField;
        
        private bool typeFieldSpecified;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        public Bonus Bonus {
            get {
                return this.bonusField;
            }
            set {
                this.bonusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Achieve", IsNullable=false)]
        public DependencyAchieve[] Dependency {
            get {
                return this.dependencyField;
            }
            set {
                this.dependencyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Category Category {
            get {
                return this.categoryField;
            }
            set {
                this.categoryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CategorySpecified {
            get {
                return this.categoryFieldSpecified;
            }
            set {
                this.categoryFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Type Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified {
            get {
                return this.typeFieldSpecified;
            }
            set {
                this.typeFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd")]
    public partial class Bonus {
        
        private uint fuelField;
        
        private uint bullField;
        
        private uint steelField;
        
        private uint bauxiteField;
        
        private uint instantBuildMaterialsField;
        
        private uint instantRepairMaterialsField;
        
        private uint developmentMaterialsField;
        
        private uint renovationMaterialsField;
        
        private string otherField;
        
        /// <remarks/>
        public uint Fuel {
            get {
                return this.fuelField;
            }
            set {
                this.fuelField = value;
            }
        }
        
        /// <remarks/>
        public uint Bull {
            get {
                return this.bullField;
            }
            set {
                this.bullField = value;
            }
        }
        
        /// <remarks/>
        public uint Steel {
            get {
                return this.steelField;
            }
            set {
                this.steelField = value;
            }
        }
        
        /// <remarks/>
        public uint Bauxite {
            get {
                return this.bauxiteField;
            }
            set {
                this.bauxiteField = value;
            }
        }
        
        /// <remarks/>
        public uint InstantBuildMaterials {
            get {
                return this.instantBuildMaterialsField;
            }
            set {
                this.instantBuildMaterialsField = value;
            }
        }
        
        /// <remarks/>
        public uint InstantRepairMaterials {
            get {
                return this.instantRepairMaterialsField;
            }
            set {
                this.instantRepairMaterialsField = value;
            }
        }
        
        /// <remarks/>
        public uint DevelopmentMaterials {
            get {
                return this.developmentMaterialsField;
            }
            set {
                this.developmentMaterialsField = value;
            }
        }
        
        /// <remarks/>
        public uint RenovationMaterials {
            get {
                return this.renovationMaterialsField;
            }
            set {
                this.renovationMaterialsField = value;
            }
        }
        
        /// <remarks/>
        public string Other {
            get {
                return this.otherField;
            }
            set {
                this.otherField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd")]
    public partial class DependencyAchieve {
        
        private string idField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd")]
    public enum Category {
        
        /// <remarks/>
        composition,
        
        /// <remarks/>
        sortie,
        
        /// <remarks/>
        practice,
        
        /// <remarks/>
        expeditions,
        
        /// <remarks/>
        supply,
        
        /// <remarks/>
        building,
        
        /// <remarks/>
        remodelling,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd")]
    public enum Type {
        
        /// <remarks/>
        onetime,
        
        /// <remarks/>
        daily,
        
        /// <remarks/>
        weekly,
        
        /// <remarks/>
        monthly,
        
        /// <remarks/>
        quarterly,
        
        /// <remarks/>
        yearly,
        
        /// <remarks/>
        other,
    }
}
