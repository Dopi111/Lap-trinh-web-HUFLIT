using Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Login.Controllers
{
    public class AccountController : Controller
    {
        UserData1Entities db = new UserData1Entities();

        // GET: Account
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        
        public ActionResult Register(UserRegister ur)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp UserName
                if (db.UserRegister.Any(x => x.UserName == ur.UserName))
                {
                    ViewBag.ErrorMessage = "User name already exists. Please choose a different user name.";
                    return View();
                }

                // Kiểm tra trùng lặp Email
                if (db.UserRegister.Any(x => x.Email == ur.Email))
                {
                    ViewBag.ErrorMessage = "Email already registered. Please use a different email address.";
                    return View();
                }

                // Nếu không trùng, thêm người dùng mới vào cơ sở dữ liệu
                db.UserRegister.Add(ur);
                db.SaveChanges();

                // Gửi thông báo thành công vào TempData
                TempData["SuccessMessage"] = "Registration successful! You will be redirected to the login page.";

                // Chuyển hướng đến trang Login
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(MyLogin l)
        {
            var query = db.UserRegister.SingleOrDefault(m => m.Email == l.Email && m.Password == l.Password);
            if (query != null)
            {
                // Thực hiện hành động sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid Credentials";
                return View();
            }
        }
    }
}