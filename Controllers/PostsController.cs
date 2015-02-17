using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
	public class PostsController : Controller
	{
		const int PostsPerPage = 10;

		public ActionResult Index(int page = 1)
		{
			var baseQuery = Database.Session.Query<Post>()
				.Where( p => p.DeletedAt == null).OrderByDescending(p => p.CreatedAt);
			int totalCount = baseQuery.Count();
			IList<int> postIds = baseQuery
				.Skip((page - 1) * PostsPerPage)
				.Take(PostsPerPage)
				.Select(p => p.Id)
				.ToList();
			IList<Post> posts = baseQuery
				.Where( p => postIds.Contains(p.Id))
				.FetchMany(f => f.Tags)
				.Fetch(f => f.User)
				.ToList();

			return View(new PostsIndex()
				{
					Posts = new Infrastructure.PagedData<Post>(posts, totalCount, page, PostsPerPage)
				});
		}

		public ActionResult Show(string idAndSlug)
		{
			System.Tuple<int, string> parts = SeperateIdAndSlug(idAndSlug);
			if (parts == null)
				return HttpNotFound();

			Post post = Database.Session.Load<Post>(parts.Item1);
			if (post == null || post.IsDeleted)
				return HttpNotFound();

			if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
				return RedirectToActionPermanent("Post", new {id=parts.Item1, slug=post.Slug});

			return View(new PostsShow() { Post = post });
		}
		public ActionResult Tag(string idAndSlug, int page = 1)
		{
			System.Tuple<int, string> parts = SeperateIdAndSlug(idAndSlug);
			if (parts == null)
				return HttpNotFound();

			Tag tag = Database.Session.Load<Tag>(parts.Item1);
			if (tag == null)
				return HttpNotFound();

			if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
				return RedirectToActionPermanent("Tag", new {id=parts.Item1, slug=tag.Slug});

			int totalPostCount = tag.Posts.Count();
			IList<int> postIds = tag.Posts
				.OrderByDescending(p => p.CreatedAt)
				.Skip((page-1) * PostsPerPage)
				.Take(PostsPerPage)
				.Where(p => p.DeletedAt == null)
				.Select(p => p.Id)
				.ToList();

			IList<Post> posts = Database.Session.Query<Post>()
				.OrderByDescending(p => p.CreatedAt)
				.Where(p => postIds.Contains(p.Id))
				.FetchMany(p => p.Tags)
				.Fetch( p => p.User)
				.ToList();

			return View(new PostsTag() 
			{ 
				Tag = tag,
				Posts = new PagedData<Post>(posts, totalPostCount, page, PostsPerPage)
			});
		}

		System.Tuple<int, string> SeperateIdAndSlug(string idAndSlug)
		{
			Match matches = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
			if (!matches.Success)
				return null;
			int id = int.Parse(matches.Result("$1"));
			string slug = matches.Result("$2");
			return Tuple.Create(id, slug);
		}
	}
}