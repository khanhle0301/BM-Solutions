namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditChiTiet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChiTietThuChis", "NgayTao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ChiTietThuChis", "TienChi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ChiTietThuChis", "TienThu", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ChiTietThuChis", "GhiChu");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChiTietThuChis", "GhiChu", c => c.String(maxLength: 100));
            AlterColumn("dbo.ChiTietThuChis", "TienThu", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ChiTietThuChis", "TienChi", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ChiTietThuChis", "NgayTao", c => c.DateTime());
        }
    }
}
