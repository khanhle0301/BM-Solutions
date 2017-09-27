namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_DuAnUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TienVonBanDaus", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.TienVonBanDaus", "DuAnId", "dbo.DuAns");
            DropIndex("dbo.TienVonBanDaus", new[] { "UserId" });
            DropIndex("dbo.TienVonBanDaus", new[] { "DuAnId" });
            AddColumn("dbo.DuAnUsers", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.DuAnUsers", "TienVonBanDau", c => c.Long(nullable: false));
            AddColumn("dbo.DuAnUsers", "IsDelete", c => c.Boolean(nullable: false));
            DropTable("dbo.TienVonBanDaus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TienVonBanDaus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DuAnId = c.String(maxLength: 50, unicode: false),
                        TongTien = c.Long(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.DuAnUsers", "IsDelete");
            DropColumn("dbo.DuAnUsers", "TienVonBanDau");
            DropColumn("dbo.DuAnUsers", "NgayTao");
            CreateIndex("dbo.TienVonBanDaus", "DuAnId");
            CreateIndex("dbo.TienVonBanDaus", "UserId");
            AddForeignKey("dbo.TienVonBanDaus", "DuAnId", "dbo.DuAns", "Id");
            AddForeignKey("dbo.TienVonBanDaus", "UserId", "dbo.AppUsers", "Id");
        }
    }
}
