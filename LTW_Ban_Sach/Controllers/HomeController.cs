using LTW_Ban_Sach.Identity;
using LTW_Ban_Sach.Models;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LTW_Ban_Sach.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            DateTime fromDate = DateTime.Now.AddMonths(-2);


            List<Books> newb = db.Books
                .Where(r => r.PublicationYear >= fromDate)
                .ToList();
            ViewBag.NewList = newb;


            List<Books> hotb = db.Books.OrderBy(r => r.LuotMua).Take(10).ToList();
            ViewBag.HotList = hotb;


            List<Books> sale = db.Books.Where(r => r.Discount > 0).ToList();
            foreach (var item in sale)
            {
                item.PriceSale = item.Price - (item.Price * (decimal)(item.Discount / 100));
            }
            db.SaveChanges();
            ViewBag.SaleList = sale;



            List<Books> bs = db.Books.ToList();
            return View(bs);
        }
        
    }



    //Cài Nutget
    //1. install-package boottstrap
    //2. install-package fontawesome
    //3. install-package entityframework
    //4. install-package microsoft.aspnet.webapi
    //5. install-package microsoft.aspnet.webapi.cors
    //6. enable-migrations
    //7. add-migration "InitialCreate"
    //8. update-database
}