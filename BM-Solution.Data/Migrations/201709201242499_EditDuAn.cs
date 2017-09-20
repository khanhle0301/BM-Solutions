namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDuAn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DuAns", "NgayTao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DuAns", "ThoiGianDuTinh", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DuAns", "LoiNhuanDuTinh", c => c.Long(nullable: false));
            AlterColumn("dbo.DuAns", "TienChiDuTinh", c => c.Long(nullable: false));
            AlterColumn("dbo.DuAns", "TienChiThucTe", c => c.Long(nullable: false));
            AlterColumn("dbo.DuAns", "TienThuDuTinh", c => c.Long(nullable: false));
            AlterColumn("dbo.DuAns", "TienThuThucTe", c => c.Long(nullable: false));
            AlterColumn("dbo.DuAns", "LoiNhuanThucTe", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DuAns", "LoiNhuanThucTe", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DuAns", "TienThuThucTe", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DuAns", "TienThuDuTinh", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DuAns", "TienChiThucTe", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DuAns", "TienChiDuTinh", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DuAns", "LoiNhuanDuTinh", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DuAns", "ThoiGianDuTinh", c => c.DateTime());
            AlterColumn("dbo.DuAns", "NgayTao", c => c.DateTime());
        }
    }
}
