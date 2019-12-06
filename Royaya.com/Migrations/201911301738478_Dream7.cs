namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dream7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "UserRating", c => c.Int(nullable: false));
            AddColumn("dbo.Dreams", "RatingDate", c => c.DateTime());
            AlterColumn("dbo.Dreams", "ExplanationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dreams", "ExplanationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Dreams", "RatingDate");
            DropColumn("dbo.Dreams", "UserRating");
        }
    }
}
