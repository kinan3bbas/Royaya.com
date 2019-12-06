namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "numbOfDreamsInOneDay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "numbOfDreamsInOneDay");
        }
    }
}
