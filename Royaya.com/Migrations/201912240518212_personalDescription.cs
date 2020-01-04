namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personalDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PersonalDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PersonalDescription");
        }
    }
}
