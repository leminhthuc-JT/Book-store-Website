using LTW_Ban_Sach.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Identity;
using Microsoft.AspNet.Identity;

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
        public ActionResult Detail(int id, int cateid)
        {
            Books b = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            List<ImagesBook> imgB = db.ImagesBooks.Where(x => x.BookId == id).ToList();
            List<Books> books = db.Books.Where(x => x.CateId == cateid && x.BookId != id).ToList();

            // Lấy danh sách đánh giá
            List<DanhGia> reviews = db.DanhGias.Where(x => x.BookId == id).OrderByDescending(x => x.ReviewDate).ToList();

            // Tính điểm trung bình
            double avgRating = 0;
            if (reviews.Count > 0)
            {
                avgRating = reviews.Average(x => x.Rating);
            }

            // Tạo Dictionary để map userId với userName
            AppDbContext userDb = new AppDbContext();
            Dictionary<string, string> userNames = new Dictionary<string, string>();
            foreach (var review in reviews)
            {
                if (!userNames.ContainsKey(review.Id))
                {
                    var user = userDb.Users.FirstOrDefault(x => x.Id == review.Id);
                    userNames[review.Id] = user?.UserName ?? "User";
                }
            }

            ViewBag.BookList = books;
            ViewBag.img = imgB;
            ViewBag.Reviews = reviews;
            ViewBag.UserNames = userNames; // Thêm Dictionary này
            ViewBag.AvgRating = Math.Round(avgRating, 1);
            ViewBag.ReviewCount = reviews.Count;

            string userId = User.Identity.GetUserId();
            AppUser u = userDb.Users.Where(x => x.Id == userId).FirstOrDefault();

            // Kiểm tra user đã đánh giá chưa
            bool hasReviewed = false;
            if (u != null)
            {
                hasReviewed = db.DanhGias.Any(x => x.BookId == id && x.Id == u.Id);
            }

            ViewBag.UserId = u?.Id;
            ViewBag.UserName = u?.UserName;
            ViewBag.HasReviewed = hasReviewed;
            ViewBag.CateId = Session["CateId"];
            ViewBag.Search = Session["Search"];
            ViewBag.Date = Session["Date"];
            ViewBag.Price = Session["Price"];
            ViewBag.Quantity = Session["Quantity"];
            ViewBag.Page = Session["Page"];

            return View(b);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddReview(int bookId, string userId, int rating, string comment, HttpPostedFileBase imageFile)
        {
            try
            {
                // Kiểm tra user đã đánh giá chưa
                var existingReview = db.DanhGias.FirstOrDefault(x => x.BookId == bookId && x.Id == userId);
                if (existingReview != null)
                {
                    TempData["Error"] = "Bạn đã đánh giá sản phẩm này rồi!";
                    return RedirectToAction("Detail", new { id = bookId, cateid = db.Books.Find(bookId).CateId });
                }

                string imageUrl = null;

                // Xử lý upload ảnh nếu có
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(imageFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Image/Reviews/"), uniqueFileName);

                    // Tạo thư mục nếu chưa có
                    string directory = System.IO.Path.GetDirectoryName(path);
                    if (!System.IO.Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }

                    imageFile.SaveAs(path);
                    imageUrl = "Reviews/" + uniqueFileName;
                }

                // Tạo đánh giá mới
                DanhGia review = new DanhGia
                {
                    BookId = bookId,
                    Id = userId,
                    Rating = rating,
                    Comment = comment,
                    ReviewDate = DateTime.Now,
                    ImageUrl = imageUrl
                };

                db.DanhGias.Add(review);
                db.SaveChanges();

                TempData["Success"] = "Đánh giá của bạn đã được gửi thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
            }

            var book = db.Books.Find(bookId);
            return RedirectToAction("Detail", new { id = bookId, cateid = book.CateId });
        }
    }
}