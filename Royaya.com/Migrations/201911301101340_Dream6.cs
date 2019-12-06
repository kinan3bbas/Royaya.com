namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dream6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "Explanation", c => c.String());
            AddColumn("dbo.Dreams", "ExplanationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dreams", "ExplanationDate");
            DropColumn("dbo.Dreams", "Explanation");
        }
    }
}
