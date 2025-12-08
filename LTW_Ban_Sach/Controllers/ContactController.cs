using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Helpers;

namespace LTW_Ban_Sach.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        // POST: Contact/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(string fullName, string email, string phone, string subject, string message)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(phone) ||
                    string.IsNullOrWhiteSpace(subject) ||
                    string.IsNullOrWhiteSpace(message))
                {
                    TempData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return RedirectToAction("Index");
                }

                // Lấy Username từ người đăng nhập
                string username = "Khách"; // Mặc định

                if (User.Identity.IsAuthenticated)
                {
                    // Nếu đã đăng nhập, lấy UserName
                    username = User.Identity.Name;
                }

                // Gửi email (gồm: fullName, email, phone, subject, message, username)
                bool emailSent = EmailHelper.SendContactEmail(
                    fullName,
                    email,
                    phone,
                    subject,
                    message,
                    username  // Truyền thêm username
                );

                if (emailSent)
                {
                    TempData["Success"] = "✅ Gửi tin nhắn thành công! Chúng tôi sẽ phản hồi trong thời gian sớm nhất.";
                }
                else
                {
                    TempData["Error"] = "❌ Có lỗi xảy ra khi gửi email. Vui lòng thử lại sau!";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi SendMessage: {ex.Message}");
                TempData["Error"] = "❌ Có lỗi xảy ra. Vui lòng thử lại sau!";
            }

            return RedirectToAction("Index");
        }
    }
}