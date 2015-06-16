namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyToAdmiralModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admirals", "MaxShipCount", c => c.Int(nullable: false, defaultValue: 100));
            AddColumn("dbo.Admirals", "Rank", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admirals", "Rank");
            DropColumn("dbo.Admirals", "MaxShipCount");
        }
    }
}
