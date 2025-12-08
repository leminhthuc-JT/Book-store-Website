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
            return View();
        }
        public ActionResult ThongKe()
        {
            DBContext db = new DBContext();
            List<Bills> bills = new List<Bills>();
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            foreach (Bills b in bills)
            {
                int tongDonHang = db.Bills
                .Where(o => o.CreateDate.Month == month && o.CreateDate.Year == year)
                .Count();
                if (tongDonHang == 0 || tongDonHang == null)
                    ViewBag.TongDonHang = 0;
                else
                    ViewBag.TongDonHang += tongDonHang;
            }

            return View();
        }
        public ActionResult BieuDo()
        {
            DBContext db = new DBContext();
            List<Bills> bills = new List<Bills>();
            List<int> DonHang = new List<int>();
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            foreach (Bills b in bills)
            {
                int Donhang = db.Bills
               .Where(o => o.CreateDate.Month == month && o.CreateDate.Year == year)
               .Count();
                DonHang.Add(Donhang);
            }
            ViewBag.DonHang = DonHang;
            return View();
        }

    }

}
//Add-Migration AddCart -ConfigurationTypeName LTW_Ban_Sach.Migrations.Configuration