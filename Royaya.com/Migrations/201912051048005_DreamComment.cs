namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DreamComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DreamComments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        DreamId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Dreams", t => t.DreamId, cascadeDelete: true)
                .Index(t => t.DreamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DreamComments", "DreamId", "dbo.Dreams");
            DropIndex("dbo.DreamComments", new[] { "DreamId" });
            DropTable("dbo.DreamComments");
        }
    }
}
