namespace Diploma_serverless.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VacationLeaves", "UserId", "dbo.Users");
            DropIndex("dbo.VacationLeaves", new[] { "UserId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.VacationLeaves", "UserId");
            AddForeignKey("dbo.VacationLeaves", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
