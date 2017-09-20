namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDuAnUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Permissions", newName: "DuAnUsers");
            DropColumn("dbo.DuAnUsers", "CanRead");
            DropColumn("dbo.DuAnUsers", "CanUpdate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DuAnUsers", "CanUpdate", c => c.Boolean(nullable: false));
            AddColumn("dbo.DuAnUsers", "CanRead", c => c.Boolean(nullable: false));
            RenameTable(name: "dbo.DuAnUsers", newName: "Permissions");
        }
    }
}
