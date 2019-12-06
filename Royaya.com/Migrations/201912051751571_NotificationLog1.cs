namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationLog1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationLogs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        UserId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UsersDeviceTokens",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        token = c.String(),
                        UserId = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationLogs", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.NotificationLogs", new[] { "User_Id" });
            DropTable("dbo.UsersDeviceTokens");
            DropTable("dbo.NotificationLogs");
        }
    }
}
