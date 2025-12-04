namespace LTW_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        Id = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookId, t.Id })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            AddColumn("dbo.Bills", "Id", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "BookId", "dbo.Books");
            DropIndex("dbo.Carts", new[] { "BookId" });
            DropColumn("dbo.Bills", "Id");
            DropTable("dbo.Carts");
        }
    }
}
