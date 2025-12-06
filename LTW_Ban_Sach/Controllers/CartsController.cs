using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Identity;
using LTW_Ban_Sach.Models;


namespace LTW_Ban_Sach.Controllers
{
    public class CartsController : Controller
    {
        // GET: Carts
        private DBContext db = new DBContext();
        public ActionResult Index(string userId = "")
        {
            List<Cart> carts = new List<Cart>();
            if (userId != null)
            {
                carts = db.Carts.Where(x=> x.Id == userId).ToList();
                
            }

            ViewBag.UserId = userId;

            decimal tongTien = carts.Sum(x => x.Quantity * x.Book.Price);
            ViewBag.TongTien = tongTien;
            return View(carts);
        }
        [HttpPost]
        public ActionResult Add(int bookId = 0, string userId = "", int quantity = 0, int CateId = 0)
        {
            if(bookId != 0 && userId != "")
            {
                Cart cartitem = db.Carts.Where(x => x.BookId == bookId).FirstOrDefault();
                if(cartitem != null)
                {
                    cartitem.Quantity += quantity;

                   
                }
                else
                {
                    Cart cart = new Cart();
                    cart.BookId = bookId;
                    cart.Id = userId;
                    cart.Quantity = quantity;
                    db.Carts.Add(cart);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Detail", "Products", new { id = bookId, cateid = CateId });
        }
        public ActionResult DeleteCart(int bookId = 0, string userId = "")
        {
            Cart cartitem = db.Carts.Where(x => x.BookId == bookId && x.Id == userId).FirstOrDefault();
            db.Carts.Remove(cartitem);
            db.SaveChanges();

            return RedirectToAction("Index", "Carts", new { userId = userId});
        }
        [HttpPost]
        public ActionResult UpdateQuantity (int bookId = 0, string userId = "", int quantity = 0)
        {
            Cart cartItem = db.Carts.Where(x => x.BookId == bookId && x.Id == userId).FirstOrDefault();
            cartItem.Quantity = quantity;
            db.SaveChanges();
            return RedirectToAction("Index", "Carts", new { userId = userId });
        }
    }
}