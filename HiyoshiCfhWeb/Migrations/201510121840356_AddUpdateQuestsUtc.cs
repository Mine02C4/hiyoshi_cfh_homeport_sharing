namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateQuestsUtc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admirals", "UpdateQuestsUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admirals", "UpdateQuestsUtc");
        }
    }
}
