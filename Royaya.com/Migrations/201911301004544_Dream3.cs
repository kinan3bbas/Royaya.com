namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dream3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dreams", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dreams", "Description", c => c.String(nullable: false));
        }
    }
}
