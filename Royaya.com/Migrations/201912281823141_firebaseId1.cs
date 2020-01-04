namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firebaseId1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "CreatorFireBaseId", c => c.String());
            AddColumn("dbo.Dreams", "InterpreterFireBaseId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dreams", "InterpreterFireBaseId");
            DropColumn("dbo.Dreams", "CreatorFireBaseId");
        }
    }
}
