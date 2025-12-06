using LTW_Ban_Sach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Identity;
using System.Runtime.CompilerServices;

namespace LTW_Ban_Sach.Controllers
{
    public class PayingController : Controller
    {
        // GET: Paying
        private DBContext db = new DBContext();
        private AppDbContext appDb = new AppDbContext();
        
        public ActionResult Index(int bookId = 0, string userId = "", int quantity = 0)
        {
            Books cart = db.Books.Where(x => x.BookId == bookId).FirstOrDefault();


            //Books b = db.Books.Where(x => x.BookId == bookId).FirstOrDefault();
            AppUser user = appDb.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (quantity == 0)
            {
                quantity = cart.Carts.Where(x => x.Id == userId).FirstOrDefault().Quantity;
            }
            ViewBag.Quantity = quantity;

            ViewBag.User = user;
            return View(cart);
        }
    }
}