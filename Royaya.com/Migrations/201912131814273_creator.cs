namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.DreamComments", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Dreams", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.InterprationPaths", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.NotificationLogs", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.UsersDeviceTokens", "CreatorUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ContactUs", "CreatorUser_Id");
            CreateIndex("dbo.Dreams", "CreatorUser_Id");
            CreateIndex("dbo.InterprationPaths", "CreatorUser_Id");
            CreateIndex("dbo.DreamComments", "CreatorUser_Id");
            CreateIndex("dbo.NotificationLogs", "CreatorUser_Id");
            CreateIndex("dbo.UsersDeviceTokens", "CreatorUser_Id");
            AddForeignKey("dbo.Dreams", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.InterprationPaths", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ContactUs", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DreamComments", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.NotificationLogs", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UsersDeviceTokens", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersDeviceTokens", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationLogs", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DreamComments", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactUs", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InterprationPaths", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dreams", "CreatorUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UsersDeviceTokens", new[] { "CreatorUser_Id" });
            DropIndex("dbo.NotificationLogs", new[] { "CreatorUser_Id" });
            DropIndex("dbo.DreamComments", new[] { "CreatorUser_Id" });
            DropIndex("dbo.InterprationPaths", new[] { "CreatorUser_Id" });
            DropIndex("dbo.Dreams", new[] { "CreatorUser_Id" });
            DropIndex("dbo.ContactUs", new[] { "CreatorUser_Id" });
            DropColumn("dbo.UsersDeviceTokens", "CreatorUser_Id");
            DropColumn("dbo.NotificationLogs", "CreatorUser_Id");
            DropColumn("dbo.InterprationPaths", "CreatorUser_Id");
            DropColumn("dbo.Dreams", "CreatorUser_Id");
            DropColumn("dbo.DreamComments", "CreatorUser_Id");
            DropColumn("dbo.ContactUs", "CreatorUser_Id");
        }
    }
}
