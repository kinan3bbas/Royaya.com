namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactUs2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactUs", "interpretatorId", "dbo.AspNetUsers");
            DropIndex("dbo.ContactUs", new[] { "interpretatorId" });
            AddColumn("dbo.ContactUs", "Email", c => c.String(nullable: false));
            AddColumn("dbo.ContactUs", "Name", c => c.String(nullable: false));
            AddColumn("dbo.ContactUs", "JobDescription", c => c.String(nullable: false));
            AddColumn("dbo.ContactUs", "PhoneNumber", c => c.String(nullable: false));
            DropColumn("dbo.ContactUs", "interpretatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactUs", "interpretatorId", c => c.String(maxLength: 128));
            DropColumn("dbo.ContactUs", "PhoneNumber");
            DropColumn("dbo.ContactUs", "JobDescription");
            DropColumn("dbo.ContactUs", "Name");
            DropColumn("dbo.ContactUs", "Email");
            CreateIndex("dbo.ContactUs", "interpretatorId");
            AddForeignKey("dbo.ContactUs", "interpretatorId", "dbo.AspNetUsers", "Id");
        }
    }
}
