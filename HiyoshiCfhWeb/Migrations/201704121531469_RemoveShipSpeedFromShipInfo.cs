namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveShipSpeedFromShipInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShipInfoes", "ShipSpeed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShipInfoes", "ShipSpeed", c => c.Int(nullable: false));
        }
    }
}
