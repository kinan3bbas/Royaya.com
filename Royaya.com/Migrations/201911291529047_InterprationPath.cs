namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterprationPath : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterprationPaths",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Cost = c.Double(nullable: false),
                        Status = c.String(),
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
            DropTable("dbo.InterprationPaths");
        }
    }
}
