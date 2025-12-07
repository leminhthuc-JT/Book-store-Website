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
        public JsonResult SendMessage(string fullName, string email, string phone, string subject, string message)
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
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng điền đầy đủ thông tin!"
                    });
                }

                // Lấy username nếu user đã đăng nhập
                string username = User.Identity.IsAuthenticated ? User.Identity.Name : "Khách";

                // Gửi email
                bool emailSent = EmailService.SendContactEmail(
                    fullName,
                    email,
                    phone,
                    subject,
                    username,
                    message
                );

                if (emailSent)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi trong thời gian sớm nhất."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Có lỗi xảy ra khi gửi email. Vui lòng thử lại sau!"
                    });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi SendMessage: {ex.Message}");

                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra. Vui lòng thử lại sau!"
                });
            }
        }
    }
}