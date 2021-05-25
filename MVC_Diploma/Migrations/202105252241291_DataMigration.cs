namespace MVC_Diploma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Counters",
                c => new
                    {
                        CounterId = c.Guid(nullable: false),
                        CounterTypeId = c.Guid(nullable: false),
                        CounterStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CounterId);
            
            CreateTable(
                "dbo.CounterTypes",
                c => new
                    {
                        CounterTypeId = c.Guid(nullable: false),
                        FirstMeterReading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SecondMeterReading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MoneyPerMeasure = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CounterTypeId);
            
            CreateTable(
                "dbo.Offices",
                c => new
                    {
                        OfficeId = c.Guid(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        OfficeStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OfficeId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Guid(nullable: false),
                        ServiceId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ManagerId = c.Guid(nullable: false),
                        RequestStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Guid(nullable: false),
                        ServiceTypeId = c.Guid(nullable: false),
                        Description = c.String(),
                        MoneyForService = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "dbo.ServiceTypes",
                c => new
                    {
                        ServiceTypeId = c.Guid(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ServiceTypeId);
            
            AddColumn("dbo.AspNetUsers", "OfficeId", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "UserStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "CounterId", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "ReputationId", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "AccountMoney", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "DateIn", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateOut", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateOut");
            DropColumn("dbo.AspNetUsers", "DateIn");
            DropColumn("dbo.AspNetUsers", "AccountMoney");
            DropColumn("dbo.AspNetUsers", "ReputationId");
            DropColumn("dbo.AspNetUsers", "CounterId");
            DropColumn("dbo.AspNetUsers", "UserStatus");
            DropColumn("dbo.AspNetUsers", "OfficeId");
            DropTable("dbo.ServiceTypes");
            DropTable("dbo.Services");
            DropTable("dbo.Requests");
            DropTable("dbo.Offices");
            DropTable("dbo.CounterTypes");
            DropTable("dbo.Counters");
        }
    }
}
