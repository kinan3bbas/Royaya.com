namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactUs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactUs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        interpretatorId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.interpretatorId)
                .Index(t => t.interpretatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactUs", "interpretatorId", "dbo.AspNetUsers");
            DropIndex("dbo.ContactUs", new[] { "interpretatorId" });
            DropTable("dbo.ContactUs");
        }
    }
}
