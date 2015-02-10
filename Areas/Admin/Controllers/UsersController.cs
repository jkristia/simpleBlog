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
    }
}