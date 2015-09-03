namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaterialRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaterialRecords",
                c => new
                    {
                        MaterialRecordId = c.Int(nullable: false, identity: true),
                        AdmiralId = c.Int(nullable: false),
                        TimeUtc = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaterialRecordId, null, false)
                .ForeignKey("dbo.Admirals", t => t.AdmiralId, cascadeDelete: true)
                .Index(t => t.AdmiralId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaterialRecords", "AdmiralId", "dbo.Admirals");
            DropIndex("dbo.MaterialRecords", new[] { "AdmiralId" });
            DropTable("dbo.MaterialRecords");
        }
    }
}
