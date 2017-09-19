namespace BM_Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    public partial class EditUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.AppUsers", "Address");
            DropColumn("dbo.AppUsers", "Avatar");
            DropColumn("dbo.AppUsers", "BirthDay");
            DropColumn("dbo.AppUsers", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Gender", c => c.Boolean());
            AddColumn("dbo.AppUsers", "BirthDay", c => c.DateTime());
            AddColumn("dbo.AppUsers", "Avatar", c => c.String());
            AddColumn("dbo.AppUsers", "Address", c => c.String(maxLength: 256));
            AlterColumn("dbo.AppUsers", "Status", c => c.Boolean());
        }
    }
}
