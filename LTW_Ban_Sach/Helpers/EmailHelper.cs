using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace LTW_Ban_Sach.Helpers
{
    public class EmailHelper
    {
        private static string GmailA = ConfigurationManager.AppSettings["GmailA"];
        private static string AppPassword = ConfigurationManager.AppSettings["AppPassword"];
        private static string GmailB = ConfigurationManager.AppSettings["GmailB"];

        // Thêm parameter username
        public static bool SendContactEmail(string fullName, string email, string phone, string subject, string message, string username)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== BẮT ĐẦU GỬI EMAIL ===");

                // Xử lý subject text
                string subjectText = GetSubjectText(subject);

                // Tạo nội dung email
                string emailBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f5f5f5; margin: 0; padding: 20px; }}
                            .email-container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                            .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
                            .header h1 {{ margin: 0; font-size: 24px; }}
                            .content {{ padding: 30px; }}
                            .info-row {{ margin-bottom: 20px; background: #f9f9f9; padding: 15px; border-radius: 8px; }}
                            .info-label {{ font-weight: bold; color: #667eea; margin-bottom: 5px; }}
                            .info-value {{ color: #333; font-size: 15px; }}
                            .message-box {{ background: #fff8e1; border-left: 4px solid #ffc107; padding: 20px; border-radius: 5px; margin-top: 20px; }}
                            .message-box h3 {{ margin-top: 0; color: #f57c00; }}
                            .footer {{ background: #f5f5f5; padding: 20px; text-align: center; color: #888; font-size: 12px; }}
                            .username-badge {{ display: inline-block; background: #4CAF50; color: white; padding: 5px 15px; border-radius: 20px; font-size: 14px; margin-top: 10px; }}
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='header'>
                                <h1>📧 TIN NHẮN MỚI TỪ WEBSITE</h1>
                                <p style='margin: 5px 0 0 0; opacity: 0.9;'>Bies BookStore Contact Form</p>
                                <div class='username-badge'>👤 Tài khoản: {username}</div>
                            </div>
                            
                            <div class='content'>
                                <div class='info-row'>
                                    <div class='info-label'>👤 Họ và Tên:</div>
                                    <div class='info-value'>{fullName}</div>
                                </div>
                                
                                <div class='info-row'>
                                    <div class='info-label'>📧 Email:</div>
                                    <div class='info-value'>{email}</div>
                                </div>
                                
                                <div class='info-row'>
                                    <div class='info-label'>📱 Số điện thoại:</div>
                                    <div class='info-value'>{phone}</div>
                                </div>
                                
                                <div class='info-row'>
                                    <div class='info-label'>📋 Chủ đề:</div>
                                    <div class='info-value'>{subjectText}</div>
                                </div>
                                
                                <div class='message-box'>
                                    <h3>💬 Nội dung tin nhắn:</h3>
                                    <p style='line-height: 1.6; color: #555; margin: 10px 0 0 0;'>{message.Replace("\n", "<br>")}</p>
                                </div>
                            </div>
                            
                            <div class='footer'>
                                <p>Email này được gửi tự động từ form liên hệ trên website Bies BookStore</p>
                                <p><strong>Người gửi:</strong> {username} {(username == "Khách" ? "(Chưa đăng nhập)" : "(Đã đăng nhập)")}</p>
                                <p>Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
                            </div>
                        </div>
                    </body>
                    </html>
                ";

                // Cấu hình email
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(GmailA, "Bies BookStore");
                mail.To.Add(GmailB);
                mail.Subject = $"[Liên hệ - {username}] {subjectText} - {fullName}";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                // SMTP
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(GmailA, AppPassword);
                smtp.Timeout = 30000;

                System.Diagnostics.Debug.WriteLine("Đang gửi email...");
                smtp.Send(mail);
                System.Diagnostics.Debug.WriteLine("✅ GỬI EMAIL THÀNH CÔNG!");

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ LỖI: {ex.Message}");
                return false;
            }
        }

        private static string GetSubjectText(string subject)
        {
            switch (subject)
            {
                case "order": return "Hỏi về đơn hàng";
                case "product": return "Thông tin sản phẩm";
                case "support": return "Hỗ trợ kỹ thuật";
                case "cooperation": return "Hợp tác kinh doanh";
                case "other": return "Khác";
                default: return subject;
            }
        }
    }
}