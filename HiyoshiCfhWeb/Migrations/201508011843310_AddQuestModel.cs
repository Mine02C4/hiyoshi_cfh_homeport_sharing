namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quests",
                c => new
                    {
                        QuestId = c.Int(nullable: false),
                        AdmiralId = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Fuel = c.Int(nullable: false),
                        Bull = c.Int(nullable: false),
                        Steel = c.Int(nullable: false),
                        Bauxite = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestId)
                .ForeignKey("dbo.Admirals", t => t.AdmiralId, cascadeDelete: true)
                .Index(t => t.AdmiralId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quests", "AdmiralId", "dbo.Admirals");
            DropIndex("dbo.Quests", new[] { "AdmiralId" });
            DropTable("dbo.Quests");
        }
    }
}
