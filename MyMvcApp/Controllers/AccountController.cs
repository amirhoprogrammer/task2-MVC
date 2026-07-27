using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = UserService.Login(username, password);

            if (user == null)
            {
                TempData["Error"] = "نام کاربری یا رمز عبور اشتباه است";
                return View();
            }

            // ذخیره اطلاعات در Session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Email", user.Email ?? "");
            HttpContext.Session.SetString("Phone", user.Phone ?? "");

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                TempData["Error"] = "نام کاربری و رمز عبور الزامی است";
                return View(user);
            }

            bool success = UserService.Register(user);
            if (!success)
            {
                TempData["Error"] = "این نام کاربری قبلاً ثبت شده است";
                return View(user);
            }

            // بعد از ثبت‌نام موفق، خودکار لاگین کن
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Email", user.Email ?? "");
            HttpContext.Session.SetString("Phone", user.Phone ?? "");

            TempData["Success"] = "ثبت‌نام با موفقیت انجام شد";
            return RedirectToAction("Profile");
        }

        public IActionResult Profile()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.Username = username;
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Phone = HttpContext.Session.GetString("Phone");

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}