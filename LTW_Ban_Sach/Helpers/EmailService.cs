using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LTW_Ban_Sach.Helpers
{
    public class EmailService
    {
        /// <summary>
        /// Gửi email liên hệ từ khách hàng
        /// Gmail A (cố định) sẽ gửi đến Gmail của Web
        /// </summary>
        public static bool SendContactEmail(string fullName, string email, string phone, string subject, string username, string message)
        {
            try
            {
                // Lấy thông tin từ Web.config
                string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];      // Gmail A (cố định)
                string senderPassword = ConfigurationManager.AppSettings["SenderPassword"]; // App Password của Gmail A
                string receiverEmail = ConfigurationManager.AppSettings["ReceiverEmail"];   // Gmail của Web

                // Tạo email message
                var mail = new MailMessage();
                mail.From = new MailAddress(senderEmail, "Book Store Contact System"); // Từ Gmail A
                mail.To.Add(receiverEmail); // Đến Gmail của Web
                mail.Subject = $"[Liên hệ] {GetSubjectText(subject)} - {fullName}";
                mail.IsBodyHtml = true;

                // Tạo nội dung email HTML đẹp
                var body = new StringBuilder();
                body.AppendLine("<!DOCTYPE html>");
                body.AppendLine("<html>");
                body.AppendLine("<head>");
                body.AppendLine("<style>");
                body.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; background: #f4f4f4; padding: 20px; margin: 0; }");
                body.AppendLine(".container { max-width: 600px; margin: 0 auto; background: white; border-radius: 10px; overflow: hidden; box-shadow: 0 0 20px rgba(0,0,0,0.1); }");
                body.AppendLine(".header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }");
                body.AppendLine(".header h2 { margin: 0; font-size: 24px; }");
                body.AppendLine(".content { padding: 30px; }");
                body.AppendLine(".info-row { margin-bottom: 20px; padding: 15px; background: #f8f9fa; border-left: 4px solid #667eea; border-radius: 5px; }");
                body.AppendLine(".label { font-weight: bold; color: #667eea; display: inline-block; min-width: 150px; }");
                body.AppendLine(".value { color: #333; }");
                body.AppendLine(".message-box { background: #fff9e6; padding: 20px; margin-top: 20px; border-radius: 8px; border: 2px solid #ffd700; }");
                body.AppendLine(".message-box h3 { margin-top: 0; color: #667eea; }");
                body.AppendLine(".footer { padding: 20px; background: #f8f9fa; text-align: center; color: #999; font-size: 12px; }");
                body.AppendLine("</style>");
                body.AppendLine("</head>");
                body.AppendLine("<body>");
                body.AppendLine("<div class='container'>");

                // Header
                body.AppendLine("<div class='header'>");
                body.AppendLine("<h2>TIN NHẮN LIÊN HỆ MỚI</h2>");
                body.AppendLine("<p>Từ khách hàng trên Website Book Store</p>");
                body.AppendLine("</div>");

                // Content
                body.AppendLine("<div class='content'>");

                // Thông tin khách hàng
                body.AppendLine("<div class='info-row'>");
                body.AppendLine($"<span class='label'>Họ và tên:</span>");
                body.AppendLine($"<span class='value'><strong>{fullName}</strong></span>");
                body.AppendLine("</div>");

                body.AppendLine("<div class='info-row'>");
                body.AppendLine($"<span class='label'>Email khách hàng:</span>");
                body.AppendLine($"<span class='value'><a href='mailto:{email}' style='color: #667eea;'>{email}</a></span>");
                body.AppendLine("</div>");

                body.AppendLine("<div class='info-row'>");
                body.AppendLine($"<span class='label'>Số điện thoại:</span>");
                body.AppendLine($"<span class='value'><strong>{phone}</strong></span>");
                body.AppendLine("</div>");

                // Tên đăng nhập (nếu có)
                if (!string.IsNullOrEmpty(username) && username != "Khách")
                {
                    body.AppendLine("<div class='info-row'>");
                    body.AppendLine($"<span class='label'>Tên đăng nhập:</span>");
                    body.AppendLine($"<span class='value'><strong>{username}</strong></span>");
                    body.AppendLine("</div>");
                }

                body.AppendLine("<div class='info-row'>");
                body.AppendLine($"<span class='label'>Chủ đề:</span>");
                body.AppendLine($"<span class='value'><strong>{GetSubjectText(subject)}</strong></span>");
                body.AppendLine("</div>");

                // Message box
                body.AppendLine("<div class='message-box'>");
                body.AppendLine("<h3>Nội dung tin nhắn:</h3>");
                body.AppendLine($"<div style='white-space: pre-wrap;'>{System.Web.HttpUtility.HtmlEncode(message)}</div>");
                body.AppendLine("</div>");

                // Hướng dẫn trả lời
                body.AppendLine("<div style='margin-top: 30px; padding: 15px; background: #e3f2fd; border-radius: 8px;'>");
                body.AppendLine("<p style='margin: 0; color: #1976d2;'><strong>Để trả lời khách hàng:</strong></p>");
                body.AppendLine($"<p style='margin: 5px 0 0 0;'>Gửi email đến: <a href='mailto:{email}' style='color: #1976d2;'>{email}</a></p>");
                body.AppendLine("</div>");

                body.AppendLine("</div>");

                // Footer
                body.AppendLine("<div class='footer'>");
                body.AppendLine($"<p>Thời gian nhận: <strong>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</strong></p>");
                body.AppendLine("<p>Email này được gửi tự động từ hệ thống Book Store</p>");
                body.AppendLine($"<p>Gửi từ: {senderEmail} → Đến: {receiverEmail}</p>");
                body.AppendLine("</div>");

                body.AppendLine("</div>");
                body.AppendLine("</body>");
                body.AppendLine("</html>");

                mail.Body = body.ToString();

                // Gửi email qua Gmail SMTP (dùng Gmail A)
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 20000; // 20 giây
                    smtp.Send(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Lỗi gửi email: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Chi tiết: {ex.StackTrace}");
                return false;
            }
        }

        private static string GetSubjectText(string subjectCode)
        {
            switch (subjectCode)
            {
                case "order":
                    return "Hỏi về đơn hàng";
                case "product":
                    return "Thông tin sản phẩm";
                case "support":
                    return "Hỗ trợ kỹ thuật";
                case "cooperation":
                    return "Hợp tác kinh doanh";
                case "other":
                    return "Khác";
                default:
                    return subjectCode;
            }
        }
    }
}