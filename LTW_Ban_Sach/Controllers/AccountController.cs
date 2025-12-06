using LTW_Ban_Sach.Identity;
using LTW_Ban_Sach.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;


namespace LTW_Ban_Sach.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Regester()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Regester(Register re)
        {
            if (ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                var passHash = Crypto.HashPassword(re.Password);
                var user = new AppUser()
                {
                    Email = re.Email,
                    UserName = re.UserName,
                    PasswordHash = passHash,
                    PhoneNumber = re.PhoneNumber,
                    Address = re.Address
                };
                IdentityResult identityResult = userManager.Create(user);
                if (identityResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");
                }
                return RedirectToAction("Login", "Account");

            }
            else { 
                return View(); 
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login lg)
        {
            if (ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                var user = userManager.Find(lg.UserName, lg.Password);
                if (user != null)
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = false }, userIdentity);
                    if (userManager.IsInRole(user.Id, "Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult ProFile(string name = "")
        {
            AppDbContext profile = new AppDbContext();
            AppUser user = profile.Users.Where(r => r.UserName == name).FirstOrDefault();
            if (user == null)
                return HttpNotFound("User not found.");
            return View(user);
        }

        public ActionResult EditProfile( string name = "")
        {
            ViewBag.PreUrl = Request.UrlReferrer?.ToString();
            AppDbContext profile = new AppDbContext();
            AppUser user = profile.Users.Where(r => r.UserName == name).FirstOrDefault();
            if (user == null)
                return HttpNotFound("User not found.");
            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(AppUser user, string preURL = "")
        {
            AppDbContext profile = new AppDbContext();
            AppUser NewUser = profile.Users.Where(r => r.UserName == user.UserName).FirstOrDefault();
            if (NewUser == null)
                return HttpNotFound("User not found.");

            NewUser.Address = user.Address;
            NewUser.PhoneNumber = user.PhoneNumber;
            NewUser.UserName = user.UserName;
            NewUser.Email = user.Email;
            profile.SaveChanges();

            return Redirect(preURL);
        }
    }
}