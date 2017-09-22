namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditThuChi : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChiTietThuChis", "TienChi", c => c.Long(nullable: false));
            AlterColumn("dbo.ChiTietThuChis", "TienThu", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChiTietThuChis", "TienThu", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ChiTietThuChis", "TienChi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
