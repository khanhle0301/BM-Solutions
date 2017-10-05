namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_DuAnUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DuAnUsers", "PhanTramVon", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DuAnUsers", "PhanTramVon");
        }
    }
}
