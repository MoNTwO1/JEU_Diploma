namespace MVC_Diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixrequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Status", c => c.String());
            AddColumn("dbo.Requests", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Description");
            DropColumn("dbo.Requests", "Status");
        }
    }
}
