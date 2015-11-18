namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSlotItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SlotItemInfoes",
                c => new
                    {
                        SlotItemInfoId = c.Int(nullable: false),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Firepower = c.Int(nullable: false),
                        Torpedo = c.Int(nullable: false),
                        AA = c.Int(nullable: false),
                        Armer = c.Int(nullable: false),
                        Bomb = c.Int(nullable: false),
                        AS = c.Int(nullable: false),
                        Hit = c.Int(nullable: false),
                        Evasiveness = c.Int(nullable: false),
                        Search = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SlotItemInfoId);
            
            CreateTable(
                "dbo.SlotItems",
                c => new
                    {
                        SlotItemUid = c.Int(nullable: false, identity: true),
                        AdmiralId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        SlotItemInfoId = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Adept = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SlotItemUid)
                .ForeignKey("dbo.Admirals", t => t.AdmiralId, cascadeDelete: true)
                .ForeignKey("dbo.SlotItemInfoes", t => t.SlotItemInfoId, cascadeDelete: true)
                .Index(t => t.AdmiralId)
                .Index(t => t.SlotItemInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SlotItems", "SlotItemInfoId", "dbo.SlotItemInfoes");
            DropForeignKey("dbo.SlotItems", "AdmiralId", "dbo.Admirals");
            DropIndex("dbo.SlotItems", new[] { "SlotItemInfoId" });
            DropIndex("dbo.SlotItems", new[] { "AdmiralId" });
            DropTable("dbo.SlotItems");
            DropTable("dbo.SlotItemInfoes");
        }
    }
}
