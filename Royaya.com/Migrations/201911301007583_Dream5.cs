namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dream5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Dreams", new[] { "interpretator_Id" });
            DropColumn("dbo.Dreams", "interpretatorId");
            RenameColumn(table: "dbo.Dreams", name: "interpretator_Id", newName: "interpretatorId");
            AlterColumn("dbo.Dreams", "interpretatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Dreams", "interpretatorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Dreams", new[] { "interpretatorId" });
            AlterColumn("dbo.Dreams", "interpretatorId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Dreams", name: "interpretatorId", newName: "interpretator_Id");
            AddColumn("dbo.Dreams", "interpretatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Dreams", "interpretator_Id");
        }
    }
}
