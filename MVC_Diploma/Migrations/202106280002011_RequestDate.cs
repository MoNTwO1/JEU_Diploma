namespace MVC_Diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Date");
        }
    }
}
