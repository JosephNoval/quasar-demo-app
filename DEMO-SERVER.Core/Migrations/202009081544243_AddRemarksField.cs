namespace DEMO_SERVER.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemarksField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Remarks", c => c.String());
            AddColumn("dbo.Items", "Remarks", c => c.String());
            AddColumn("dbo.Orders", "Remarks", c => c.String());
            AddColumn("dbo.OrderDetails", "Remarks", c => c.String());
            AddColumn("dbo.Users", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Remarks");
            DropColumn("dbo.OrderDetails", "Remarks");
            DropColumn("dbo.Orders", "Remarks");
            DropColumn("dbo.Items", "Remarks");
            DropColumn("dbo.Customers", "Remarks");
        }
    }
}
