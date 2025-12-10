using LTW_Ban_Sach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Identity;

namespace LTW_Ban_Sach.Controllers
{
    public class PayingController : Controller
    {
        private DBContext db = new DBContext();
        private AppDbContext appDb = new AppDbContext();

        // GET: Paying (Giữ nguyên code cũ của bạn)
        public ActionResult Index(int bookId = 0, string userId = "", int quantity = 0)
        {
            Books cart = db.Books.Where(x => x.BookId == bookId).FirstOrDefault();
            AppUser user = appDb.Users.Where(x => x.Id == userId).FirstOrDefault();

            // Logic lấy số lượng từ giỏ hàng nếu không truyền vào
            if (quantity == 0)
            {
                // Lưu ý: Cần check null ở đây để tránh lỗi nếu user chưa có giỏ hàng
                var userCart = cart.Carts.Where(x => x.Id == userId).FirstOrDefault();
                quantity = userCart != null ? userCart.Quantity : 1;
            }

            ViewBag.Quantity = quantity;
            ViewBag.User = user;
            return View(cart);
        }

        // POST: Paying/ProcessOrder - XỬ LÝ ĐẶT HÀNG
        [HttpPost]
        public ActionResult ProcessOrder(int bookId, string userId, int quantity, decimal price, string paymentMethod)
        {
            try
            {
                // 1. Tạo hóa đơn (Bill)
                Bills newBill = new Bills();
                newBill.Id = userId;
                newBill.CreateDate = DateTime.Now;
                newBill.TotalAmount = price * quantity;
                newBill.PaymentMethod = paymentMethod;

                // --- ĐÃ XÓA DÒNG newBill.VoucherId = ... ---
                // Vì bên Model đã cho phép null, nên ta không cần gán giá trị gì cả,
                // hệ thống sẽ tự hiểu là null.

                db.Bills.Add(newBill);
                db.SaveChanges(); // Lưu Bill trước để lấy BillId

                // 2. Tạo chi tiết hóa đơn
                DetailBills detail = new DetailBills();
                detail.BillId = newBill.BillId;
                detail.BookId = bookId;
                detail.Quantity = quantity;

                db.DetailBills.Add(detail);
                db.SaveChanges();

                // 3. Thành công -> Chuyển hướng
                return RedirectToAction("Index", "Orders", new { userId = userId });
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                // Bắt lỗi Validation (ví dụ: thiếu trường bắt buộc, sai định dạng)
                string msg = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += $"Lỗi tại '{validationError.PropertyName}': {validationError.ErrorMessage} <br/>";
                    }
                }
                return Content("<h3>LỖI DỮ LIỆU (VALIDATION):</h3>" + msg);
            }
            catch (Exception ex)
            {
                // Bắt lỗi SQL hoặc lỗi hệ thống khác
                Exception currentEx = ex;
                // Vòng lặp để moi ra cái lỗi sâu nhất trong cùng (Inner Exception)
                while (currentEx.InnerException != null)
                {
                    currentEx = currentEx.InnerException;
                }

                return Content($"<h3>LỖI HỆ THỐNG GỐC:</h3> <p>{currentEx.Message}</p> <p><strong>Stack Trace:</strong> {currentEx.StackTrace}</p>");
            }
        }
    }
}