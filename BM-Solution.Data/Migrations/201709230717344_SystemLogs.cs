namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.String(maxLength: 50, unicode: false),
                        NgayTao = c.DateTime(nullable: false),
                        NoiDung = c.String(maxLength: 500),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemLogs");
        }
    }
}
