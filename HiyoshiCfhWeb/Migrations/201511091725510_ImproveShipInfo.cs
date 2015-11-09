namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImproveShipInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipInfoes", "Kana", c => c.String());
            AddColumn("dbo.ShipInfoes", "MaxHp", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxFuel", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxBull", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxFirepower", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxTorpedo", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxAA", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxArmer", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MaxLuck", c => c.Int(nullable: false));
            AddColumn("dbo.ShipInfoes", "MinLuck", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipInfoes", "MinLuck");
            DropColumn("dbo.ShipInfoes", "MaxLuck");
            DropColumn("dbo.ShipInfoes", "MaxArmer");
            DropColumn("dbo.ShipInfoes", "MaxAA");
            DropColumn("dbo.ShipInfoes", "MaxTorpedo");
            DropColumn("dbo.ShipInfoes", "MaxFirepower");
            DropColumn("dbo.ShipInfoes", "MaxBull");
            DropColumn("dbo.ShipInfoes", "MaxFuel");
            DropColumn("dbo.ShipInfoes", "MaxHp");
            DropColumn("dbo.ShipInfoes", "Kana");
        }
    }
}
