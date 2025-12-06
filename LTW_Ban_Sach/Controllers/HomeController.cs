using LTW_Ban_Sach.Identity;
using LTW_Ban_Sach.Models;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW_Ban_Sach.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            List<Books> bs = db.Books.ToList();
            return View(bs);
        }
        
    }



    //Cài Nutget
    //1. install-package boottstrap
    //2. install-package fontawesome
    //3. install-package entityframework
    //4. install-package microsoft.aspnet.webapi
    //5. install-package microsoft.aspnet.webapi.cors
    //6. enable-migrations
    //7. add-migration "InitialCreate"
    //8. update-database
}