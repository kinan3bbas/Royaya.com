namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creator1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "CreatorId", c => c.String());
            AddColumn("dbo.Dreams", "CreatorId", c => c.String());
            AddColumn("dbo.InterprationPaths", "CreatorId", c => c.String());
            AddColumn("dbo.DreamComments", "CreatorId", c => c.String());
            AddColumn("dbo.NotificationLogs", "CreatorId", c => c.String());
            AddColumn("dbo.UsersDeviceTokens", "CreatorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersDeviceTokens", "CreatorId");
            DropColumn("dbo.NotificationLogs", "CreatorId");
            DropColumn("dbo.DreamComments", "CreatorId");
            DropColumn("dbo.InterprationPaths", "CreatorId");
            DropColumn("dbo.Dreams", "CreatorId");
            DropColumn("dbo.ContactUs", "CreatorId");
        }
    }
}
