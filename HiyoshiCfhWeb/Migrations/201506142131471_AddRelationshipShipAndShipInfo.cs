namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationshipShipAndShipInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ships", "ShipInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ships", "ShipInfoId");
            AddForeignKey("dbo.Ships", "ShipInfoId", "dbo.ShipInfoes", "ShipInfoId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ships", "ShipInfoId", "dbo.ShipInfoes");
            DropIndex("dbo.Ships", new[] { "ShipInfoId" });
            DropColumn("dbo.Ships", "ShipInfoId");
        }
    }
}
