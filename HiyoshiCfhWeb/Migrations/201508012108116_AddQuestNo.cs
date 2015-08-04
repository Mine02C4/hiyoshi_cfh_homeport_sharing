namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quests", "QuestNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Quests", "QuestId", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quests", "QuestId", c => c.Int(nullable: false));
            DropColumn("dbo.Quests", "QuestNo");
        }
    }
}
