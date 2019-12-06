namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dream : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dreams",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        interpretatorId = c.Int(nullable: false),
                        pathId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        Modifier = c.String(),
                        AttachmentId = c.Int(nullable: false),
                        interpretator_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.interpretator_Id)
                .ForeignKey("dbo.InterprationPaths", t => t.pathId, cascadeDelete: true)
                .Index(t => t.pathId)
                .Index(t => t.interpretator_Id);
            
            AlterColumn("dbo.InterprationPaths", "Status", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dreams", "pathId", "dbo.InterprationPaths");
            DropForeignKey("dbo.Dreams", "interpretator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Dreams", new[] { "interpretator_Id" });
            DropIndex("dbo.Dreams", new[] { "pathId" });
            AlterColumn("dbo.InterprationPaths", "Status", c => c.String());
            DropTable("dbo.Dreams");
        }
    }
}
