namespace VisionMelbourneV3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmergencyContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmergencyContact", c => c.String());
            DropColumn("dbo.AspNetUsers", "TextSize");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TextSize", c => c.String());
            DropColumn("dbo.AspNetUsers", "EmergencyContact");
        }
    }
}
