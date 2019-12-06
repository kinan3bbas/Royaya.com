namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dream1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dreams", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dreams", "Status", c => c.String(nullable: false));
        }
    }
}
