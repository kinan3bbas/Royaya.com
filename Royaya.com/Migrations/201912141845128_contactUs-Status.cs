namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactUsStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUs", "Status");
        }
    }
}
