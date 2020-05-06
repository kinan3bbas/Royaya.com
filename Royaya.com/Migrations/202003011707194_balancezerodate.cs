namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class balancezerodate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "balancezerodate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "balancezerodate");
        }
    }
}
