namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publicInterpreterRatio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PublicInterpreterRatios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ratio = c.Double(nullable: false),
                        pathId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.InterprationPaths", t => t.pathId, cascadeDelete: true)
                .Index(t => t.pathId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublicInterpreterRatios", "pathId", "dbo.InterprationPaths");
            DropIndex("dbo.PublicInterpreterRatios", new[] { "pathId" });
            DropTable("dbo.PublicInterpreterRatios");
        }
    }
}
