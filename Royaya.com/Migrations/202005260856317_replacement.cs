namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replacement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Replacements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        OldinterpretatorId = c.String(maxLength: 128),
                        NewinterpretatorId = c.String(maxLength: 128),
                        DreamId = c.Int(nullable: false),
                        Reason = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Dreams", t => t.DreamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.NewinterpretatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.OldinterpretatorId)
                .Index(t => t.OldinterpretatorId)
                .Index(t => t.NewinterpretatorId)
                .Index(t => t.DreamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Replacements", "OldinterpretatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Replacements", "NewinterpretatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Replacements", "DreamId", "dbo.Dreams");
            DropIndex("dbo.Replacements", new[] { "DreamId" });
            DropIndex("dbo.Replacements", new[] { "NewinterpretatorId" });
            DropIndex("dbo.Replacements", new[] { "OldinterpretatorId" });
            DropTable("dbo.Replacements");
        }
    }
}
