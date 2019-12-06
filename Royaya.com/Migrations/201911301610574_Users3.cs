namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "LastModificationDate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "JoiningDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "JoiningDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "LastModificationDate", c => c.DateTime(nullable: false));
        }
    }
}
