namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class verifyInterpreter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "verifiedInterpreter", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "verifiedInterpreter");
        }
    }
}
