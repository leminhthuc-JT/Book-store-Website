namespace LTW_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addluotmua : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "LuotMua", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "LuotMua");
        }
    }
}
