namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class speed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CreationDate", c => c.DateTime());
        }
    }
}
