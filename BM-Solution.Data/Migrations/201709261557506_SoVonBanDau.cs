namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoVonBanDau : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.UserId)
                .ForeignKey("dbo.DuAns", t => t.DuAnId)
                .Index(t => t.UserId)
                .Index(t => t.DuAnId);
            
            AddColumn("dbo.DuAns", "TienVonBanDau", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TienVonBanDaus", "DuAnId", "dbo.DuAns");
            DropForeignKey("dbo.TienVonBanDaus", "UserId", "dbo.AppUsers");
            DropIndex("dbo.TienVonBanDaus", new[] { "DuAnId" });
            DropIndex("dbo.TienVonBanDaus", new[] { "UserId" });
            DropColumn("dbo.DuAns", "TienVonBanDau");
            DropTable("dbo.TienVonBanDaus");
        }
    }
}
