using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LTW_Ban_Sach.Models;

namespace LTW_Ban_Sach.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/Order
        public ActionResult Index()
        {
            // Load kèm thông tin sách và voucher để hiển thị
            var list = db.Bills.Include(b => b.DetailBills).Include(b => b.Vouchers).OrderByDescending(x => x.CreateDate).ToList();
            return View(list);
        }

        // --- 1. CHỨC NĂNG XEM CHI TIẾT (DETAILS) ---
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Tìm bill theo id, lấy kèm chi tiết sách và voucher
            Bills bill = db.Bills.Include(b => b.DetailBills.Select(d => d.Books)).Include(b => b.Vouchers).FirstOrDefault(b => b.BillId == id);

            if (bill == null) return HttpNotFound();

            return View(bill);
        }

        // --- 2. CHỨC NĂNG XÓA (DELETE) ---
        // GET: Hiển thị trang xác nhận xóa
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Bills bill = db.Bills.Find(id);
            if (bill == null) return HttpNotFound();
            return View(bill);
        }

        // POST: Thực hiện xóa thực sự
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bills bill = db.Bills.Find(id);

            // Quan trọng: Phải xóa các dòng trong DetailBills (chi tiết đơn hàng) trước
            // nếu không sẽ bị lỗi khóa ngoại (Foreign Key)
            var details = db.DetailBills.Where(d => d.BillId == id).ToList();
            db.DetailBills.RemoveRange(details);

            // Sau đó mới xóa Bill
            db.Bills.Remove(bill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}