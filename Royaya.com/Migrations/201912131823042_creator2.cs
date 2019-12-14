namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creator2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dreams", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InterprationPaths", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactUs", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DreamComments", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationLogs", "CreatorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsersDeviceTokens", "CreatorUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ContactUs", new[] { "CreatorUser_Id" });
            DropIndex("dbo.Dreams", new[] { "CreatorUser_Id" });
            DropIndex("dbo.InterprationPaths", new[] { "CreatorUser_Id" });
            DropIndex("dbo.DreamComments", new[] { "CreatorUser_Id" });
            DropIndex("dbo.NotificationLogs", new[] { "CreatorUser_Id" });
            DropIndex("dbo.UsersDeviceTokens", new[] { "CreatorUser_Id" });
            DropColumn("dbo.ContactUs", "CreatorId");
            DropColumn("dbo.ContactUs", "CreatorUser_Id");
            DropColumn("dbo.Dreams", "CreatorId");
            DropColumn("dbo.Dreams", "CreatorUser_Id");
            DropColumn("dbo.InterprationPaths", "CreatorId");
            DropColumn("dbo.InterprationPaths", "CreatorUser_Id");
            DropColumn("dbo.DreamComments", "CreatorId");
            DropColumn("dbo.DreamComments", "CreatorUser_Id");
            DropColumn("dbo.NotificationLogs", "CreatorId");
            DropColumn("dbo.NotificationLogs", "CreatorUser_Id");
            DropColumn("dbo.UsersDeviceTokens", "CreatorId");
            DropColumn("dbo.UsersDeviceTokens", "CreatorUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsersDeviceTokens", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.UsersDeviceTokens", "CreatorId", c => c.String());
            AddColumn("dbo.NotificationLogs", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.NotificationLogs", "CreatorId", c => c.String());
            AddColumn("dbo.DreamComments", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.DreamComments", "CreatorId", c => c.String());
            AddColumn("dbo.InterprationPaths", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.InterprationPaths", "CreatorId", c => c.String());
            AddColumn("dbo.Dreams", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Dreams", "CreatorId", c => c.String());
            AddColumn("dbo.ContactUs", "CreatorUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.ContactUs", "CreatorId", c => c.String());
            CreateIndex("dbo.UsersDeviceTokens", "CreatorUser_Id");
            CreateIndex("dbo.NotificationLogs", "CreatorUser_Id");
            CreateIndex("dbo.DreamComments", "CreatorUser_Id");
            CreateIndex("dbo.InterprationPaths", "CreatorUser_Id");
            CreateIndex("dbo.Dreams", "CreatorUser_Id");
            CreateIndex("dbo.ContactUs", "CreatorUser_Id");
            AddForeignKey("dbo.UsersDeviceTokens", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.NotificationLogs", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DreamComments", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ContactUs", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.InterprationPaths", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Dreams", "CreatorUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
