namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quests", "QuestNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quests", "QuestNo");
        }
    }
}
