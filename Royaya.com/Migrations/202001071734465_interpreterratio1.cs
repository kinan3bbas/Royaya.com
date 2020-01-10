namespace Royaya.com.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interpreterratio1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterpreterRatios", "pathId", c => c.Int(nullable: false));
            CreateIndex("dbo.InterpreterRatios", "pathId");
            AddForeignKey("dbo.InterpreterRatios", "pathId", "dbo.InterprationPaths", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterpreterRatios", "pathId", "dbo.InterprationPaths");
            DropIndex("dbo.InterpreterRatios", new[] { "pathId" });
            DropColumn("dbo.InterpreterRatios", "pathId");
        }
    }
}
