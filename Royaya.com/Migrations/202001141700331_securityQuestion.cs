namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class securityQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SecurityQuestion", c => c.String());
            AddColumn("dbo.AspNetUsers", "SecurityQuestionAnswer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SecurityQuestionAnswer");
            DropColumn("dbo.AspNetUsers", "SecurityQuestion");
        }
    }
}
