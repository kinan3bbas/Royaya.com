namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastModificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Sex", c => c.String());
            AddColumn("dbo.AspNetUsers", "Country", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Type", c => c.String());
            AddColumn("dbo.AspNetUsers", "Status", c => c.String());
            AddColumn("dbo.AspNetUsers", "MartialStatus", c => c.String());
            AddColumn("dbo.AspNetUsers", "JobDescription", c => c.String());
            AddColumn("dbo.AspNetUsers", "JoiningDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "PictureId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PictureId");
            DropColumn("dbo.AspNetUsers", "JoiningDate");
            DropColumn("dbo.AspNetUsers", "JobDescription");
            DropColumn("dbo.AspNetUsers", "MartialStatus");
            DropColumn("dbo.AspNetUsers", "Status");
            DropColumn("dbo.AspNetUsers", "Type");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "Country");
            DropColumn("dbo.AspNetUsers", "Sex");
            DropColumn("dbo.AspNetUsers", "BirthDate");
            DropColumn("dbo.AspNetUsers", "LastModificationDate");
            DropColumn("dbo.AspNetUsers", "CreationDate");
        }
    }
}
