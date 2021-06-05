namespace MVC_Diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class req : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Counters");
            DropPrimaryKey("dbo.Offices");
            DropPrimaryKey("dbo.Requests");
            DropPrimaryKey("dbo.Services");
            DropPrimaryKey("dbo.ServiceTypes");
            CreateTable(
                "dbo.Reputations",
                c => new
                    {
                        ReputationId = c.String(nullable: false, maxLength: 128),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ReputationId);
            
            AlterColumn("dbo.Counters", "CounterId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Counters", "CounterTypeId", c => c.String());
            AlterColumn("dbo.Offices", "OfficeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Requests", "RequestId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Requests", "ServiceId", c => c.String());
            AlterColumn("dbo.Requests", "UserId", c => c.String());
            AlterColumn("dbo.Requests", "ManagerId", c => c.String());
            AlterColumn("dbo.Services", "ServiceId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Services", "ServiceTypeId", c => c.String());
            AlterColumn("dbo.ServiceTypes", "ServiceTypeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "OfficeId", c => c.String());
            AlterColumn("dbo.AspNetUsers", "CounterId", c => c.String());
            AlterColumn("dbo.AspNetUsers", "ReputationId", c => c.String());
            AddPrimaryKey("dbo.Counters", "CounterId");
            AddPrimaryKey("dbo.Offices", "OfficeId");
            AddPrimaryKey("dbo.Requests", "RequestId");
            AddPrimaryKey("dbo.Services", "ServiceId");
            AddPrimaryKey("dbo.ServiceTypes", "ServiceTypeId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ServiceTypes");
            DropPrimaryKey("dbo.Services");
            DropPrimaryKey("dbo.Requests");
            DropPrimaryKey("dbo.Offices");
            DropPrimaryKey("dbo.Counters");
            AlterColumn("dbo.AspNetUsers", "ReputationId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUsers", "CounterId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUsers", "OfficeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ServiceTypes", "ServiceTypeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Services", "ServiceTypeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Services", "ServiceId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Requests", "ManagerId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Requests", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Requests", "ServiceId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Requests", "RequestId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Offices", "OfficeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Counters", "CounterTypeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Counters", "CounterId", c => c.Guid(nullable: false));
            DropTable("dbo.Reputations");
            AddPrimaryKey("dbo.ServiceTypes", "ServiceTypeId");
            AddPrimaryKey("dbo.Services", "ServiceId");
            AddPrimaryKey("dbo.Requests", "RequestId");
            AddPrimaryKey("dbo.Offices", "OfficeId");
            AddPrimaryKey("dbo.Counters", "CounterId");
        }
    }
}
