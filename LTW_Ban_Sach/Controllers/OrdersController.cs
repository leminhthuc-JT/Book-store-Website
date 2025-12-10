using LTW_Ban_Sach.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity; // Cần dòng này để dùng .Include

namespace LTW_Ban_Sach.Controllers
{
    public class OrdersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Orders
        public ActionResult Index(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách Bill của User đó, kèm theo chi tiết sách
            var listBills = db.Bills
                              .Where(b => b.Id == userId)
                              .OrderByDescending(b => b.CreateDate)
                              .ToList();

            return View(listBills);
        }
    }
}