namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiTietThuChis", "MoreImages", c => c.String(storeType: "xml"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChiTietThuChis", "MoreImages");
        }
    }
}
