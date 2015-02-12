using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;
using NHibernate.Linq;

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
			User user = Database.Session.Query<User>().FirstOrDefault( u => u.Username == login.Username);
			if (user == null)
				SimpleBlog.Models.User.FakeHash();

			if (user == null || user.CheckPassword(login.Password) == false)
				ModelState.AddModelError("Username", "Username or password is incorrect!");

			if (ModelState.IsValid == false)
				return View(login);

			FormsAuthentication.SetAuthCookie(user.Username, true);

			if (string.IsNullOrWhiteSpace(returnUrl) == false)
				return Redirect(returnUrl);

			return RedirectToRoute("home");

			//login.TestProp = "POST - " + DateTime.Now.ToLongTimeString();
			//return View(login);
		}
    }
}