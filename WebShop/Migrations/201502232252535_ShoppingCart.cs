namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40),
                        ProductId = c.String(maxLength: 40),
                        Quantity = c.Int(nullable: false),
                        Date = c.DateTime(),
                        Cart_Id = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cart", t => t.Cart_Id)
                .Index(t => t.Cart_Id);
            
            CreateTable(
                "dbo.Cart",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40),
                        Date = c.DateTime(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cart", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartItem", "Cart_Id", "dbo.Cart");
            DropIndex("dbo.Cart", new[] { "User_Id" });
            DropIndex("dbo.CartItem", new[] { "Cart_Id" });
            DropTable("dbo.Cart");
            DropTable("dbo.CartItem");
        }
    }
}
