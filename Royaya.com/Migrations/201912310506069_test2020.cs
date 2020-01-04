namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "Response", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUs", "Response");
        }
    }
}
