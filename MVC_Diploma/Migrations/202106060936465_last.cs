namespace MVC_Diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class last : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reputations", "NumberOfVotes", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "UserMark", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "UserMark");
            DropColumn("dbo.Reputations", "NumberOfVotes");
        }
    }
}
