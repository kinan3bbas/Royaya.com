namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifcationLog2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NotificationLogs", new[] { "User_Id" });
            DropColumn("dbo.NotificationLogs", "UserId");
            RenameColumn(table: "dbo.NotificationLogs", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.NotificationLogs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.NotificationLogs", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotificationLogs", new[] { "UserId" });
            AlterColumn("dbo.NotificationLogs", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.NotificationLogs", name: "UserId", newName: "User_Id");
            AddColumn("dbo.NotificationLogs", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.NotificationLogs", "User_Id");
        }
    }
}
