namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddUpdateUtcToAdmiralModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admirals", "UpdateUtc", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
        }

        public override void Down()
        {
            DropColumn("dbo.Admirals", "UpdateUtc");
        }
    }
}
