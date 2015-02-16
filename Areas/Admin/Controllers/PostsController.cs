using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using SimpleBlog.Infrastructure.Extensions;
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
				.Skip((page - 1) * PostsPerPage)
				.Take(PostsPerPage)
				.ToList();

			PostsIndex posts = new PostsIndex();
			posts.Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage);
			return View(posts);
		}

		public ActionResult New()
		{
			return View("form", new PostsForm
				{
					IsNew = true,
					Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox()
					{
						Id = tag.Id,
						Name=tag.Name,
						IsChecked = false,
					}).ToList()
				});
		}

		public ActionResult Edit(int id)
		{
			Post post = Database.Session.Load<Post>(id);
			if (post == null)
				return HttpNotFound();
			return View("form", new PostsForm()
				{
					IsNew = false,
					PostId = id,
					Title = post.Title,
					Slug = post.Slug,
					Content = post.Content,
					Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox()
					{
						Id = tag.Id,
						Name=tag.Name,
						IsChecked = post.Tags.Contains(tag),
					}).ToList()
				});
		}

		private IEnumerable<Tag> ReconsileTags(IEnumerable<TagCheckbox> tags)
		{
			foreach (TagCheckbox tag in tags.Where(t => t.IsChecked))
			{ 
				if (tag.Id != null)
				{ 
					yield return Database.Session.Load<Tag>(tag.Id);
					continue;
				}
				Tag existingTag = Database.Session.Query<Tag>().FirstOrDefault(t => t.Name == tag.Name);
				if (existingTag != null)
				{ 
					yield return existingTag;
					continue;
				}

				Tag newtag = new Tag()
				{
					Name = tag.Name,
					Slug = tag.Name.Slugify()
				};
				Database.Session.Save(newtag);
				yield return newtag;
			}
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Form(PostsForm form)
		{
			form.IsNew = (form.PostId == null);
			if (!ModelState.IsValid)
				return View(form);

			IEnumerable<Tag> selectedTags = ReconsileTags(form.Tags);

			Post post;
			if (form.IsNew)
			{
				post = new Post()
				{
					CreatedAt = DateTime.UtcNow,
					User = Auth.User
				};
			}
			else
			{
				post = Database.Session.Load<Post>(form.PostId);
				if (post == null)
					return HttpNotFound();

				post.UpdatedAt = DateTime.UtcNow;
			}
			post.Tags.Clear();
			foreach (Tag tag in selectedTags)
				post.Tags.Add(tag);

			post.Title = form.Title;
			post.Slug = form.Slug;
			post.Content = form.Content;

			Database.Session.SaveOrUpdate(post);
			return RedirectToAction("index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Trash(int id)
		{
			Post post = Database.Session.Load<Post>(id);
			if (post == null)
				return HttpNotFound();
			post.DeletedAt = DateTime.UtcNow;
			Database.Session.Update(post);
			return RedirectToAction("index");
		}
		
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Restore(int id)
		{
			Post post = Database.Session.Load<Post>(id);
			if (post == null)
				return HttpNotFound();
			post.DeletedAt = null;
			Database.Session.Update(post);
			return RedirectToAction("index");
		}
		
		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			Post post = Database.Session.Load<Post>(id);
			if (post == null)
				return HttpNotFound();
			Database.Session.Delete(post);
			return RedirectToAction("index");
		}
	}
}