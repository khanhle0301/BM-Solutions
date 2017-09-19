namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Permission : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Functions", "ParentId", "dbo.Functions");
            DropForeignKey("dbo.Permissions", "RoleId", "dbo.AppRoles");
            DropForeignKey("dbo.Permissions", "FunctionId", "dbo.Functions");
            DropIndex("dbo.Functions", new[] { "ParentId" });
            DropIndex("dbo.Permissions", new[] { "RoleId" });
            DropIndex("dbo.Permissions", new[] { "FunctionId" });
            AddColumn("dbo.Permissions", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Permissions", "DuaAnId", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.Permissions", "UserId");
            CreateIndex("dbo.Permissions", "DuaAnId");
            AddForeignKey("dbo.Permissions", "UserId", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Permissions", "DuaAnId", "dbo.DuAns", "Id");
            DropColumn("dbo.Permissions", "RoleId");
            DropColumn("dbo.Permissions", "FunctionId");
            DropColumn("dbo.Permissions", "CanCreate");
            DropColumn("dbo.Permissions", "CanDelete");
            DropTable("dbo.Functions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Url = c.String(nullable: false, maxLength: 256),
                        DisplayOrder = c.Int(nullable: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Permissions", "CanDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permissions", "CanCreate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permissions", "FunctionId", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.Permissions", "RoleId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Permissions", "DuaAnId", "dbo.DuAns");
            DropForeignKey("dbo.Permissions", "UserId", "dbo.AppUsers");
            DropIndex("dbo.Permissions", new[] { "DuaAnId" });
            DropIndex("dbo.Permissions", new[] { "UserId" });
            DropColumn("dbo.Permissions", "DuaAnId");
            DropColumn("dbo.Permissions", "UserId");
            CreateIndex("dbo.Permissions", "FunctionId");
            CreateIndex("dbo.Permissions", "RoleId");
            CreateIndex("dbo.Functions", "ParentId");
            AddForeignKey("dbo.Permissions", "FunctionId", "dbo.Functions", "Id");
            AddForeignKey("dbo.Permissions", "RoleId", "dbo.AppRoles", "Id");
            AddForeignKey("dbo.Functions", "ParentId", "dbo.Functions", "Id");
        }
    }
}
