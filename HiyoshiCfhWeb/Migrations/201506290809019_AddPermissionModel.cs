namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPermissionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        PermissionId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PermissionId);
        }

        public override void Down()
        {
            DropTable("dbo.Permissions");
        }
    }
}
