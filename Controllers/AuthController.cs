using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
		[HttpGet]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToRoute("home");
		}
		[HttpPost]
		public ActionResult Login(AuthLogin login, string returnUrl)
		{
			if (ModelState.IsValid == false)
			{ 
				login.TestProp = "NOT VALID";
				return View(login);
			}
			/*
			if (login.Username != "jesper")
			{
				ModelState.AddModelError("Username", "Username or password incorrect");
				return View(login);
			}
			*/
			// Authentication - this tells ASP.NET that the user is really the user
			// Authorization - this is done in the Role provider, and returns the roles for a given user. 
			FormsAuthentication.SetAuthCookie(login.Username, true);

			if (string.IsNullOrWhiteSpace(returnUrl) == false)
				return Redirect(returnUrl);

			return RedirectToRoute("home");

			//login.TestProp = "POST - " + DateTime.Now.ToLongTimeString();
			//return View(login);
		}
    }
}