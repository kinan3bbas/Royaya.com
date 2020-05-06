namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dreamcost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "PaidDream", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dreams", "PaidDream");
        }
    }
}
