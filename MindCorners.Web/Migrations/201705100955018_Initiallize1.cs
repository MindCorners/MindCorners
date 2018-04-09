namespace MindCorners.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initiallize1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "OrganizationId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "OrganizationId", c => c.Guid(nullable: false));
        }
    }
}
