using LTW_Ban_Sach.Identity;
using LTW_Ban_Sach.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTW_Ban_Sach.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private UserManager<AppUser> userManager;

        public UsersController()
        {
            userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));
        }
        // GET: Admin/Users
        // ---------------- DANH SÁCH ----------------
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            return View(users);
        }
        

        // ---------------- THÊM USER ----------------
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Register re)
        {
            if (ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                //var passHash = Crypto.HashPassword(re.Password);
                var user = new AppUser()
                {
                    Email = re.Email,
                    UserName = re.UserName,
                    //PasswordHash = passHash,
                    PhoneNumber = re.PhoneNumber,
                    Address = re.Address
                };
                IdentityResult identityResult = userManager.Create(user, re.Password);

                if (identityResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");
                }
                return RedirectToAction("Index");

            }
            else
            {
                return View();
            }
        }


        // ---------------- SỬA USER ----------------
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(AppUser model)
        {
            var user = db.Users.Find(model.Id);

            user.Address = model.Address;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Avatar = model.Avatar;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ---------------- XÓA USER ----------------
        public ActionResult Delete(string id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);   // trả user sang View
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                userManager.Delete(user);
            }

            return RedirectToAction("Index");
        }


    }
}