namespace LTW_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDanhGia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetailBills", "Books_BookId", "dbo.Books");
            DropIndex("dbo.DetailBills", new[] { "Books_BookId" });
            DropPrimaryKey("dbo.DetailBills");
            DropColumn("dbo.DetailBills", "BookId");
            RenameColumn(table: "dbo.DetailBills", name: "Books_BookId", newName: "BookId");
            AlterColumn("dbo.DetailBills", "BookId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DetailBills", new[] { "BillId", "BookId" });
            CreateIndex("dbo.DetailBills", "BookId");
            AddForeignKey("dbo.DetailBills", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
            CreateTable(
                "dbo.DanhGias",
                c => new
                {
                    BookId = c.Int(nullable: false),
                    Id = c.String(nullable: false, maxLength: 128),
                    Comment = c.String(nullable: false),
                    Rating = c.Int(nullable: false),
                    ReviewDate = c.DateTime(nullable: false),
                    ImageUrl = c.String(),
                })
                .PrimaryKey(t => new { t.BookId, t.Id })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            AlterColumn("dbo.Bills", "Id", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.DetailBills", "BookId", "dbo.Books");
            DropForeignKey("dbo.DanhGias", "BookId", "dbo.Books");
            DropIndex("dbo.DanhGias", new[] { "BookId" });
            DropIndex("dbo.DetailBills", new[] { "BookId" });
            DropPrimaryKey("dbo.DetailBills");
            AlterColumn("dbo.DetailBills", "BookId", c => c.Int());
            DropTable("dbo.DanhGias");
            AddPrimaryKey("dbo.DetailBills", new[] { "BillId", "BookId" });
            RenameColumn("dbo.DetailBills", "BookId", "Books_BookId");
            AddColumn("dbo.DetailBills", "BookId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DetailBills", "Books_BookId");
            AddForeignKey("dbo.DetailBills", "Books_BookId", "dbo.Books", "BookId");
        }
    }
}
