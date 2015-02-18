using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.ViewModels;
using SimpleBlog.Models;

namespace SimpleBlog.Controllers
{
    public class LayoutController : Controller
    {
		[ChildActionOnly] // <-- this prevent the url to be public accesible, can only be accessed from a view
        public ActionResult Sidebar()
        {
            return View( new LayoutSidebar
				{
					IsLoggedIn = Auth.User != null,
					Username = Auth.User != null ? Auth.User.Username : "",
					IsAdmin = User.IsInRole("admin"),
					Tags = Database.Session.Query<Tag>()
						.Select(tag => new 
						{
							tag.Id,
							tag.Name,
							tag.Slug,
							PostCount = tag.Posts.Count
						})
						.Where(a => a.PostCount > 0)
						.OrderByDescending(a => a.PostCount)
						.Select(a => new SidebarTag(a.Id, a.Name, a.Slug, a.PostCount))
						.ToList()
				});
        }
    }
}