namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationshipAdmiralToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admirals", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Admirals", "UserId");
            AddForeignKey("dbo.Admirals", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Admirals", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Admirals", new[] { "UserId" });
            DropColumn("dbo.Admirals", "UserId");
        }
    }
}
