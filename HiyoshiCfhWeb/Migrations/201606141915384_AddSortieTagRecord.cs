namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSortieTagRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SortieTagRecords",
                c => new
                    {
                        ShipUid = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        SortieTagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShipUid, t.EventId })
                .ForeignKey("dbo.Ships", t => t.ShipUid, cascadeDelete: true)
                .Index(t => t.ShipUid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SortieTagRecords", "ShipUid", "dbo.Ships");
            DropIndex("dbo.SortieTagRecords", new[] { "ShipUid" });
            DropTable("dbo.SortieTagRecords");
        }
    }
}
