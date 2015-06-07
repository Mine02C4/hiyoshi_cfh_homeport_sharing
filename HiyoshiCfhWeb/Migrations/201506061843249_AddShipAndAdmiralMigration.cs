namespace HiyoshiCfhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipAndAdmiralMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admirals",
                c => new
                    {
                        AdmiralId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Experience = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdmiralId);
            
            CreateTable(
                "dbo.Ships",
                c => new
                    {
                        ShipUid = c.Int(nullable: false, identity: true),
                        AdmiralId = c.Int(nullable: false),
                        ShipId = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Exp = c.Int(nullable: false),
                        ExpForNextLevel = c.Int(nullable: false),
                        Hp = c.Int(nullable: false),
                        Fuel = c.Int(nullable: false),
                        Bull = c.Int(nullable: false),
                        Firepower = c.Int(nullable: false),
                        Torpedo = c.Int(nullable: false),
                        AA = c.Int(nullable: false),
                        Armer = c.Int(nullable: false),
                        Luck = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShipUid)
                .ForeignKey("dbo.Admirals", t => t.AdmiralId, cascadeDelete: true)
                .Index(t => t.AdmiralId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ShipInfoes",
                c => new
                    {
                        ShipInfoId = c.Int(nullable: false, identity: true),
                        SortId = c.Int(nullable: false),
                        Name = c.String(),
                        ShipTypeId = c.Int(nullable: false),
                        ShipSpeed = c.Int(nullable: false),
                        NextRemodelingLevel = c.Int(),
                    })
                .PrimaryKey(t => t.ShipInfoId)
                .ForeignKey("dbo.ShipTypes", t => t.ShipTypeId, cascadeDelete: true)
                .Index(t => t.ShipTypeId);
            
            CreateTable(
                "dbo.ShipTypes",
                c => new
                    {
                        ShipTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SortNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShipTypeId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShipInfoes", "ShipTypeId", "dbo.ShipTypes");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ships", "AdmiralId", "dbo.Admirals");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ShipInfoes", new[] { "ShipTypeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ships", new[] { "AdmiralId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ShipTypes");
            DropTable("dbo.ShipInfoes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Ships");
            DropTable("dbo.Admirals");
        }
    }
}
