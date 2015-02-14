using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;

namespace SimpleBlog.Areas.Admin.Controllers
{
	[Authorize(Roles = "admin")]
	[SelectedTab("posts")]
    public class PostsController : Controller
    {
		const int PostsPerPage = 5;

        public ActionResult Index(int page = 1)
        {
			int totalPostCount = Database.Session.Query<Post>().Count();
			IList<Post> currentPostPage = Database.Session.Query<Post>()
				.OrderByDescending(p => p.CreatedAt)
				.Skip((page - 1 ) * PostsPerPage)
				.Take(PostsPerPage)
				.ToList();

			PostsIndex posts = new PostsIndex();
			posts.Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage);
            return View(posts);
        }
    }
}