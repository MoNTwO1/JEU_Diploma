namespace MVC_Diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class admits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "MasterAdmit", c => c.String());
            AddColumn("dbo.Requests", "ManagerAdmit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "ManagerAdmit");
            DropColumn("dbo.Requests", "MasterAdmit");
        }
    }
}
