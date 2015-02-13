using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Infrastructure;
using NHibernate.Linq;
using SimpleBlog.Models;

namespace SimpleBlog.Areas.Admin.Controllers
{
	[Authorize(Roles = "admin")]
	[SelectedTab("users")]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
			UsersIndex users = new UsersIndex { Users = Database.Session.Query<User>().ToList() };
            return View(users);
        }

		public ActionResult New()
		{
			return View(new UsersNew()
			{
				Roles = Database.Session.Query<Role>().Select( role => new RoleCheckbox() 
				{
					Id = role.Id,
					IsChecked = false,
					Name = role.Name
				}).ToList()
			});
		}
		
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult New(UsersNew newuser)
		{
			User user = new User();
			SyncRoles(newuser.Roles, user.Roles);
			if (Database.Session.Query<User>().Any(u => u.Username == newuser.Username))
				ModelState.AddModelError("Username", "Username must be unique");
			if (ModelState.IsValid == false)
				return View(newuser);

			user.Username = newuser.Username;
			user.Email = newuser.Email;
			user.SetPassword(newuser.Password);
			// transaction is defined globally in TransactionFilter
			Database.Session.Save(user);
			return RedirectToAction("index");
		}
		public ActionResult Edit(int id)
		{
			User user = Database.Session.Load<User>(id);
			if (user == null)
				return HttpNotFound();
			return View(new UsersEdit()
			{
				Username = user.Username,
				Email = user.Email,
				Roles = Database.Session.Query<Role>().Select( role => new RoleCheckbox() 
				{
					Id = role.Id,
					IsChecked = user.Roles.Contains(role),
					Name = role.Name
				}).ToList()
			});
		}
		
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(int id, UsersEdit form)
		{
			User user = Database.Session.Load<User>(id);
			if (user == null)
				return HttpNotFound();

			SyncRoles(form.Roles, user.Roles);

			if (Database.Session.Query<User>().Any( u => u.Username == form.Username && u.Id != id))
				ModelState.AddModelError("Username", "Username must be unique");

			if (ModelState.IsValid == false)
				return View(form);

			user.Username = form.Username;
			user.Email = form.Email;
			Database.Session.Update(user);
			return RedirectToAction("index");
		}
		public ActionResult ResetPassword(int id)
		{
			User user = Database.Session.Load<User>(id);
			if (user == null)
				return HttpNotFound();
			return View(new UsersResetPassword
			{
				Username = user.Username,
			});
		}
		
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult ResetPassword(int id, UsersResetPassword form)
		{
			User user = Database.Session.Load<User>(id);
			if (user == null)
				return HttpNotFound();

			// this so user name appear on the view
			form.Username = user.Username;
			if (ModelState.IsValid == false)
				return View(form);

			user.SetPassword(form.Password);
			Database.Session.Update(user);
			return RedirectToAction("index");
		}
		
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			User user = Database.Session.Load<User>(id);
			if (user == null)
				return HttpNotFound();
			Database.Session.Delete(user);
			return RedirectToAction("index");
		}

		private void SyncRoles(IList<RoleCheckbox> checkboxes, IList<Role> roles)
		{
			List<Role> selectedRoles = new List<Role>();
			foreach (Role role in Database.Session.Query<Role>())
			{
				RoleCheckbox cb = checkboxes.Single(c => c.Id == role.Id);
				cb.Name = role.Name;
				if (cb.IsChecked)
					selectedRoles.Add(role);
			}
			foreach (Role toAdd in selectedRoles.Where(r => roles.Contains(r) == false))
				roles.Add(toAdd);
			foreach (Role toRemove in roles.Where(r => selectedRoles.Contains(r) == false).ToArray())
				roles.Remove(toRemove);
		}
    }
}