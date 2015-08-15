namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSortieTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ships", "SortieTag", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ships", "SortieTag");
        }
    }
}
