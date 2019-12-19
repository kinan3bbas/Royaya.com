namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numberOfViewsz : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "numberOfViews", c => c.Long(nullable: false));
            AddColumn("dbo.Dreams", "numberOfLikes", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dreams", "numberOfLikes");
            DropColumn("dbo.Dreams", "numberOfViews");
        }
    }
}
