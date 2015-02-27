namespace WebShop.Erp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ErpModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40),
                        ProductId = c.String(maxLength: 40),
                        Price = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Order_Id = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40),
                        Date = c.DateTime(),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Title = c.Int(),
                        Address = c.String(maxLength: 50),
                        House = c.String(maxLength: 50),
                        Zip = c.String(maxLength: 10),
                        City = c.String(maxLength: 50),
                        Tax = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "Order_Id" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
        }
    }
}
