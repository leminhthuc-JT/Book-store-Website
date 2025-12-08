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
using System.IO;
using LTW_Ban_Sach.Models;
using System.Web.UI.WebControls;

namespace LTW_Ban_Sach.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();
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
        public ActionResult Login(LoginModel lg)
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
                    ModelState.AddModelError("", "Tên hoặc mật khẩu không đúng.");
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
        public ActionResult ProFile(string userId = "")
        {
            AppDbContext profile = new AppDbContext();
            AppUser user = profile.Users.Where(r => r.Id == userId).FirstOrDefault();
            if (user == null)
                return HttpNotFound("User not found.");
            return View(user);
        }

        public ActionResult EditProfile( string userId = "")
        {
            ViewBag.PreUrl = Request.UrlReferrer?.ToString();
            AppDbContext profile = new AppDbContext();
            AppUser user = profile.Users.Where(r => r.Id == userId).FirstOrDefault();
            ViewBag.UserId = user.Id;
            if (user == null)
                return HttpNotFound("User not found.");
            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(AppUser user, string preURL = "")
        {
            AppDbContext profile = new AppDbContext();
            AppUser NewUser = profile.Users.Where(r => r.Id == user.Id).FirstOrDefault();
            if (NewUser == null)
                return HttpNotFound("User not found.");

            NewUser.Address = user.Address;
            NewUser.PhoneNumber = user.PhoneNumber;
            NewUser.UserName = user.UserName;
            NewUser.Email = user.Email;
            profile.SaveChanges();

            return Redirect(preURL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAvatar(HttpPostedFileBase avatar, string userId)
        {
            try
            {
                if (avatar == null || avatar.ContentLength == 0)
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn ảnh!";
                    return RedirectToAction("ProFile", new { userId = userId });
                }

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(avatar.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    TempData["ErrorMessage"] = "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif)!";
                    return RedirectToAction("ProFile", new { userId = userId });
                }

                // Kiểm tra kích thước file (max 5MB)
                if (avatar.ContentLength > 5 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "Kích thước ảnh không được vượt quá 5MB!";
                    return RedirectToAction("ProFile", new { userId = userId });
                }

                // Tạo tên file unique
                var fileName = Guid.NewGuid().ToString() + extension;
                var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);

                // Lưu file
                avatar.SaveAs(path);

                // Cập nhật database
                var user = db.Users.Find(userId); // Thay db bằng DbContext của bạn
                if (user != null)
                {
                    // Xóa ảnh cũ nếu không phải ảnh mặc định
                    if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar != "default-avatar.png")
                    {
                        var oldImagePath = Path.Combine(Server.MapPath("~/Content/Image"), user.Avatar);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Cập nhật avatar mới
                    user.Avatar = fileName;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật avatar thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction("ProFile", new { userId = userId });
        }

        public ActionResult ChangePassWord(string userId = "")
        {
            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassWord(ChangePasswordModel cpw)
        {
            if (!ModelState.IsValid)
                return View();

            string userId = User.Identity.GetUserId();

            var appDBContext = new AppDbContext();
            var userStore = new AppUserStore(appDBContext);
            var userManager = new AppUserManager(userStore);

            var user = userManager.FindById(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Không tìm thấy user!");
                return View();
            }

            // Kiểm tra mật khẩu cũ
            if (!userManager.CheckPassword(user, cpw.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return View();
            }

            // Hash đúng chuẩn
            var passwordHasher = new PasswordHasher();
            user.PasswordHash = passwordHasher.HashPassword(cpw.NewPassword);

            // Lưu user
            IdentityResult result = userManager.Update(user);

            // BẮT BUỘC phải check lỗi
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.FirstOrDefault());
                return View();
            }

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Login", "Account");
        }

    }
}