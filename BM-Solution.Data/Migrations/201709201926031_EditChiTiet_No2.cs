namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditChiTiet_No2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChiTietThuChis", name: "DuaAnId", newName: "DuAnId");
            RenameIndex(table: "dbo.ChiTietThuChis", name: "IX_DuaAnId", newName: "IX_DuAnId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ChiTietThuChis", name: "IX_DuAnId", newName: "IX_DuaAnId");
            RenameColumn(table: "dbo.ChiTietThuChis", name: "DuAnId", newName: "DuaAnId");
        }
    }
}
