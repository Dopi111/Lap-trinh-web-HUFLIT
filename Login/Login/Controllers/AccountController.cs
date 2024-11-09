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
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Register(UserRegister ur)
        {
            if (ModelState.IsValid)
            {
                if (db.UserRegister.Any(x => x.Email == ur.Email))
                {
                    ViewBag.Mesager = "Email already registered";
                }
                else
                {
                    db.UserRegister.Add(ur);
                    db.SaveChanges();
                    Response.Write("<script>alert('Registration successful')<script>");

                }

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
                Response.Write("<script>alert('Login successful')<script>");
            }
            else
            {
                Response.Write("<script>alert('Invalid Credentials')<script>");
            }
            return View();
        }
    }
}