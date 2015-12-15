namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaxHp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ships", "MaxHp", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ships", "MaxHp");
        }
    }
}
