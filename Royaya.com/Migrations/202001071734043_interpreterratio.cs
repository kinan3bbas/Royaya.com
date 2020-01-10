namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interpreterratio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterpreterRatios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        interpretatorId = c.String(maxLength: 128),
                        ratio = c.Double(nullable: false),
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
            DropForeignKey("dbo.InterpreterRatios", "interpretatorId", "dbo.AspNetUsers");
            DropIndex("dbo.InterpreterRatios", new[] { "interpretatorId" });
            DropTable("dbo.InterpreterRatios");
        }
    }
}
