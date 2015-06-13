namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAutoIncrement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShipInfoes", "ShipTypeId", "dbo.ShipTypes");
            DropTable("dbo.ShipTypes");
            CreateTable(
                "dbo.ShipTypes",
                c => new
                {
                    ShipTypeId = c.Int(nullable: false),
                    Name = c.String(),
                    SortNumber = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ShipTypeId);
            AddForeignKey("dbo.ShipInfoes", "ShipTypeId", "dbo.ShipTypes", "ShipTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShipInfoes", "ShipTypeId", "dbo.ShipTypes");
            DropTable("dbo.ShipTypes");
            CreateTable(
                "dbo.ShipTypes",
                c => new
                {
                    ShipTypeId = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    SortNumber = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ShipTypeId);
            AddForeignKey("dbo.ShipInfoes", "ShipTypeId", "dbo.ShipTypes", "ShipTypeId", cascadeDelete: true);
        }
    }
}
