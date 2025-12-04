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
        public ActionResult Index(string name)
        {
            List<Cart> carts = new List<Cart>();
            if (name != null)
            {
                carts = db.Carts.Where(x=> x.Id == name).ToList();
                
            }
            return View(carts);
        }
        [HttpPost]
        public ActionResult Add(int bookId = 0, string name = "", int quantity = 0, int CateId = 0)
        {
            if(bookId != 0 && name != "")
            {
                Cart cartitem = db.Carts.Where(x => x.BookId == bookId).FirstOrDefault();
                if(cartitem != null)
                {
                    cartitem.Quantity += 1;

                   
                }
                else
                {
                    Cart cart = new Cart();
                    cart.BookId = bookId;
                    cart.Id = name;
                    cart.Quantity = quantity;
                    db.Carts.Add(cart);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Detail", "Products", new { id = bookId, cateid = CateId });
        }

    }
}