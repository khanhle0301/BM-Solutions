namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsDeleteCT : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ChiTietThuChis", "IsDelete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChiTietThuChis", "IsDelete", c => c.Boolean(nullable: false));
        }
    }
}
