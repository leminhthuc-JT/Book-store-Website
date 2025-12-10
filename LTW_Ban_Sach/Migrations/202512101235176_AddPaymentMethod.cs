namespace LTW_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentMethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "PaymentMethod", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "PaymentMethod");
        }
    }
}
