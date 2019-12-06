namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactUs1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "interpretatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ContactUs", "interpretatorId");
            AddForeignKey("dbo.ContactUs", "interpretatorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactUs", "interpretatorId", "dbo.AspNetUsers");
            DropIndex("dbo.ContactUs", new[] { "interpretatorId" });
            DropColumn("dbo.ContactUs", "interpretatorId");
        }
    }
}
