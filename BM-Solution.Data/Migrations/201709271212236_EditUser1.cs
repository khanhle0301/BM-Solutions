namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUser1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "FullName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppUsers", "FullName", c => c.String(maxLength: 256));
        }
    }
}
