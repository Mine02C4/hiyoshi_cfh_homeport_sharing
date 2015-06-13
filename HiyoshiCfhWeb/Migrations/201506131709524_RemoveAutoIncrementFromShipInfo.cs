namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAutoIncrementFromShipInfo : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ShipInfoes");
            CreateTable(
                "dbo.ShipInfoes",
                c => new
                {
                    ShipInfoId = c.Int(nullable: false),
                    SortId = c.Int(nullable: false),
                    Name = c.String(),
                    ShipTypeId = c.Int(nullable: false),
                    ShipSpeed = c.Int(nullable: false),
                    NextRemodelingLevel = c.Int(),
                })
                .PrimaryKey(t => t.ShipInfoId)
                .ForeignKey("dbo.ShipTypes", t => t.ShipTypeId, cascadeDelete: true)
                .Index(t => t.ShipTypeId);
        }
        
        public override void Down()
        {
            DropTable("dbo.ShipInfoes");
            CreateTable(
                "dbo.ShipInfoes",
                c => new
                {
                    ShipInfoId = c.Int(nullable: false, identity: true),
                    SortId = c.Int(nullable: false),
                    Name = c.String(),
                    ShipTypeId = c.Int(nullable: false),
                    ShipSpeed = c.Int(nullable: false),
                    NextRemodelingLevel = c.Int(),
                })
                .PrimaryKey(t => t.ShipInfoId)
                .ForeignKey("dbo.ShipTypes", t => t.ShipTypeId, cascadeDelete: true)
                .Index(t => t.ShipTypeId);
        }
    }
}
