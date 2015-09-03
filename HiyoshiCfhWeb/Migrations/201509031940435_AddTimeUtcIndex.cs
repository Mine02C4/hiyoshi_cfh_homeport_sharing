namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeUtcIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.MaterialRecords", "TimeUtc", clustered: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.MaterialRecords", new[] { "TimeUtc" });
        }
    }
}
