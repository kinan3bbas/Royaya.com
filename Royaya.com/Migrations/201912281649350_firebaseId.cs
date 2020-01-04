namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firebaseId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FireBaseId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FireBaseId");
        }
    }
}
