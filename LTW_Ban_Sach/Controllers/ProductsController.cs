using LTW_Ban_Sach.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace LTW_Ban_Sach.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private DBContext db = new DBContext();
        public ActionResult Index(int id = 0, string search = "", int page = 1, int date = 0, int price = 0, int quantity = 0)
        {
            List<Books> books = new List<Books>();
            if (id != 0)
            {
                books = db.Books.Where(x => x.CateId == id).ToList();
                if (!string.IsNullOrEmpty(search))
                {
                    books = books.Where(x => x.BookName.ToLower().Contains(search.ToLower()) || x.Categories.CateName.ToLower().Contains(search.ToLower())).ToList();
                    if (price == 1)
                    {
                        books = books.OrderBy(x => x.Price).ToList();
                    }
                    else if (price == -1)
                    {
                        books = books.OrderByDescending(x => x.Price).ToList();
                    }
                    if (date == 1)
                    {
                        books = books.OrderBy(x => x.PublicationYear).ToList();
                    }
                    else if (date == -1)
                    {
                        books = books.OrderByDescending(x => x.PublicationYear).ToList();
                    }
                    if (quantity == 1)
                    {
                        books = books.OrderBy(x => x.LuotMua).ToList();
                    }
                    else if (quantity == -1)
                    {
                        books = books.OrderByDescending(x => x.LuotMua).ToList();
                    }
                    
                }
                else
                {
                    if (price == 1)
                    {
                        books = books.OrderBy(x => x.Price).ToList();
                    }
                    else if (price == -1)
                    {
                        books = books.OrderByDescending(x => x.Price).ToList();
                    }
                    if (date == 1)
                    {
                        books = books.OrderBy(x => x.PublicationYear).ToList();
                    }
                    else if (date == -1)
                    {
                        books = books.OrderByDescending(x => x.PublicationYear).ToList();
                    }
                    if (quantity == 1)
                    {
                        books = books.OrderBy(x => x.LuotMua).ToList();
                    }
                    else if (quantity == -1)
                    {
                        books = books.OrderByDescending(x => x.LuotMua).ToList();
                    }
                }
            }
            else
            {
                books = db.Books.ToList();
                if (!string.IsNullOrEmpty(search))
                {
                    books = books.Where(x => x.BookName.ToLower().Contains(search.ToLower()) || x.Categories.CateName.ToLower().Contains(search.ToLower())).ToList();
                    if (price == 1)
                    {
                        books = books.OrderBy(x => x.Price).ToList();
                    }
                    else if (price == -1)
                    {
                        books = books.OrderByDescending(x => x.Price).ToList();
                    }
                    if (date == 1)
                    {
                        books = books.OrderBy(x => x.PublicationYear).ToList();
                    }
                    else if (date == -1)
                    {
                        books = books.OrderByDescending(x => x.PublicationYear).ToList();
                    }
                    if (quantity == 1)
                    {
                        books = books.OrderBy(x => x.LuotMua).ToList();
                    }
                    else if (quantity == -1)
                    {
                        books = books.OrderByDescending(x => x.LuotMua).ToList();
                    }
                }
                else
                {
                    if (date != 0 || price != 0 || quantity != 0)
                    {
                        if (price == 1)
                        {
                            books = books.OrderBy(x => x.Price).ToList();
                        }
                        else if (price == -1)
                        {
                            books = books.OrderByDescending(x => x.Price).ToList();
                        }
                        if (date == 1)
                        {
                            books = books.OrderBy(x => x.PublicationYear).ToList();
                        }
                        else if (date == -1)
                        {
                            books = books.OrderByDescending(x => x.PublicationYear).ToList();
                        }
                        if (quantity == 1)
                        {
                            books = books.OrderBy(x => x.LuotMua).ToList();
                        }
                        else if (quantity == -1)
                        {
                            books = books.OrderByDescending(x => x.LuotMua).ToList();
                        }
                    }
                }
            }
            int temp = 20;
            int pageNumber = (int)Math.Ceiling(((double)books.Count() / temp));
            books = books.Skip((page - 1) * temp).Take(temp).ToList();
            ViewBag.PageNumber = pageNumber;
            ViewBag.Page = page;

            List<Categories> cates = db.Categories.ToList();
            ViewBag.Cate = cates;
            ViewBag.Search = search;
            ViewBag.CateId = id;
            ViewBag.Date = date;
            ViewBag.Price = price;
            ViewBag.Quantity = quantity;

            Session["CateId"] = id;
            Session["Search"] = search;
            Session["Date"] = date;
            Session["Price"] = price;
            Session["Quantity"] = quantity;
            Session["Page"] = page;
            return View(books);
        }
        public ActionResult Detail (int id, int cateid)
        {
            Books b = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            List<ImagesBook> imgB = db.ImagesBooks.Where(x=>x.BookId == id).ToList();
            List<Books> books = db.Books.Where(x => x.CateId == cateid && x.BookId != id).ToList();

            ViewBag.BookList = books;
            ViewBag.img = imgB;

            ViewBag.CateId = Session["CateId"];
            ViewBag.Search = Session["Search"];
            ViewBag.Date = Session["Date"];
            ViewBag.Price = Session["Price"];
            ViewBag.Quantity = Session["Quantity"];
            ViewBag.Page = Session["Page"];

            return View(b);
        }

    }
}