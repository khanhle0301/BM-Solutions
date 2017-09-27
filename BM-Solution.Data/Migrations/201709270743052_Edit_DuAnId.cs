namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_DuAnId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DuAnUsers", name: "DuaAnId", newName: "DuAnId");
            RenameIndex(table: "dbo.DuAnUsers", name: "IX_DuaAnId", newName: "IX_DuAnId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DuAnUsers", name: "IX_DuAnId", newName: "IX_DuaAnId");
            RenameColumn(table: "dbo.DuAnUsers", name: "DuAnId", newName: "DuaAnId");
        }
    }
}
