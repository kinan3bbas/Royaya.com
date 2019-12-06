namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dream8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "InterpretationStartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dreams", "InterpretationStartDate");
        }
    }
}
