namespace WebShop.Erp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Title : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Title", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Title", c => c.Int());
        }
    }
}
