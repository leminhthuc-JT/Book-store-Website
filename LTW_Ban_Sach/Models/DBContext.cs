using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LTW_Ban_Sach.Models
{
    public class DBContext:DbContext
    {
        public DBContext() : base("MyConnectionString") {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Bills> Bills { get; set; }
        public DbSet<DetailBills> DetailBills { get; set; }
        public DbSet<ImagesBook> ImagesBooks { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<ImageEvents> ImageEvents { get; set; }
        public DbSet<Vouchers> Vouchers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
    }
}