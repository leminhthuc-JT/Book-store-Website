namespace LTW_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeVoucherNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bills", "VoucherId", "dbo.Vouchers");
            DropIndex("dbo.Bills", new[] { "VoucherId" });
            AlterColumn("dbo.Bills", "VoucherId", c => c.Int());
            CreateIndex("dbo.Bills", "VoucherId");
            AddForeignKey("dbo.Bills", "VoucherId", "dbo.Vouchers", "VoucherId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "VoucherId", "dbo.Vouchers");
            DropIndex("dbo.Bills", new[] { "VoucherId" });
            AlterColumn("dbo.Bills", "VoucherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Bills", "VoucherId");
            AddForeignKey("dbo.Bills", "VoucherId", "dbo.Vouchers", "VoucherId", cascadeDelete: true);
        }
    }
}
