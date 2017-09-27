namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDuAnUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DuAnUsers", "PhanTramHoaHong", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DuAnUsers", "PhanTramHoaHong");
        }
    }
}
