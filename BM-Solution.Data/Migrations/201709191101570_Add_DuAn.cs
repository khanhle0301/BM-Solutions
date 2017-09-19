namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DuAn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietThuChis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DuaAnId = c.String(maxLength: 50, unicode: false),
                        NgayTao = c.DateTime(),
                        TienChi = c.Decimal(precision: 18, scale: 2),
                        TienThu = c.Decimal(precision: 18, scale: 2),
                        GhiChu = c.String(maxLength: 100),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.UserId)
                .ForeignKey("dbo.DuAns", t => t.DuaAnId)
                .Index(t => t.UserId)
                .Index(t => t.DuaAnId);
            
            CreateTable(
                "dbo.DuAns",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, unicode: false),
                        Ten = c.String(nullable: false, maxLength: 100),
                        NgayTao = c.DateTime(),
                        ThoiGianDuTinh = c.DateTime(),
                        LoiNhuanDuTinh = c.Decimal(precision: 18, scale: 2),
                        TienChiDuTinh = c.Decimal(precision: 18, scale: 2),
                        TienChiThucTe = c.Decimal(precision: 18, scale: 2),
                        TienThuDuTinh = c.Decimal(precision: 18, scale: 2),
                        TienThuThucTe = c.Decimal(precision: 18, scale: 2),
                        LoiNhuanThucTe = c.Decimal(precision: 18, scale: 2),
                        NoiDung = c.String(maxLength: 500),
                        GhiChu = c.String(maxLength: 100),
                        TrangThai = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiTietThuChis", "DuaAnId", "dbo.DuAns");
            DropForeignKey("dbo.ChiTietThuChis", "UserId", "dbo.AppUsers");
            DropIndex("dbo.ChiTietThuChis", new[] { "DuaAnId" });
            DropIndex("dbo.ChiTietThuChis", new[] { "UserId" });
            DropTable("dbo.DuAns");
            DropTable("dbo.ChiTietThuChis");
        }
    }
}
