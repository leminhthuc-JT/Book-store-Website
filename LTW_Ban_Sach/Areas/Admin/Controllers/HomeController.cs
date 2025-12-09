using LTW_Ban_Sach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW_Ban_Sach.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            DBContext db = new DBContext();
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            ViewBag.month = month;
            ViewBag.year = year;
            int tongDonHang = 0;
            int DonHang = 0;
            List<Bills> b = db.Bills.Where(r => r.CreateDate.Month == month && r.CreateDate.Year == year).ToList();
            if (b != null)
            {
                tongDonHang = b.Count();
            }
            ViewBag.TongDonHang = tongDonHang;
            List<int> SoLuongDon = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                List<Bills> dsbills = db.Bills.Where(o => o.CreateDate.Month == i && o.CreateDate.Year == year).ToList();
                if (b != null)
                {
                    DonHang = b.Count();
                }
                SoLuongDon.Add(DonHang);
                ViewBag.SoLuongDon = SoLuongDon;
            }


            return View();
        }

    }

}
//Add-Migration AddCart -ConfigurationTypeName LTW_Ban_Sach.Migrations.Configuration