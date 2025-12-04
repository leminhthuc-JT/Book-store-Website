using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Models;

namespace LTW_Ban_Sach.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private DBContext db = new DBContext();
        public ActionResult Index(int id = 0, string search = "")
        {
            List<Books> books;
            if (id != 0)
            {
                books = db.Books.Where(x => x.CateId == id).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(search))
                {
                    books = db.Books.Where(x => x.BookName.Contains(search)).ToList();
                }
                else
                {
                    books = db.Books.ToList();
                }
            }

            List<Categories> cates = db.Categories.ToList();
            ViewBag.Cate = cates;

            return View(books);
        }
        public ActionResult Detail (int id, int cateid)
        {
            Books b = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            List<ImagesBook> imgB = db.ImagesBooks.Where(x=>x.BookId == id).ToList();
            List<Books> books = db.Books.Where(x => x.CateId == cateid && x.BookId != id).ToList();
            ViewBag.BookList = books;
            ViewBag.img = imgB;
            return View(b);
        }

    }
}