namespace DEMO_SERVER.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullUpdatedOn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "UpdatedOn", c => c.DateTime());
            AlterColumn("dbo.Items", "UpdatedOn", c => c.DateTime());
            AlterColumn("dbo.Orders", "UpdatedOn", c => c.DateTime());
            AlterColumn("dbo.OrderDetails", "UpdatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "UpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "UpdatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrderDetails", "UpdatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "UpdatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Items", "UpdatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "UpdatedOn", c => c.DateTime(nullable: false));
        }
    }
}
