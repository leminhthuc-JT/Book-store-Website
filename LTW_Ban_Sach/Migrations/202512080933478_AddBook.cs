namespace LTW_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Discount", c => c.Single(nullable: false));
            AddColumn("dbo.Books", "PriceSale", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "PriceSale");
            DropColumn("dbo.Books", "Discount");
        }
    }
}
