using LTW_Ban_Sach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW_Ban_Sach.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Admin/Order
        public ActionResult Index()
        {
            List<Bills> list = db.Bills.ToList();

            return View(list);
        }


    }

}