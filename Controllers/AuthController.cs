using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    public class AuthController : Controller
    {
		[HttpGet]
		public ActionResult Login()
		{
			AuthLogin m = new AuthLogin();
			m.Username = "123";
			m.TestProp = "GET - " + DateTime.Now.ToLongTimeString();
			return View(m);
		}
		[HttpPost]
		public ActionResult Login(AuthLogin login)
		{
			if (ModelState.IsValid == false)
			{ 
				login.TestProp = "NOT VALID";
				return View(login);
			}
			if (login.Username != "jesper")
			{
				ModelState.AddModelError("Username", "Username or password incorrect");
				return View(login);
			}

			login.TestProp = "POST - " + DateTime.Now.ToLongTimeString();
			return View(login);
		}
    }
}