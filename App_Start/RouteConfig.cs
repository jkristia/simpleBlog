using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleBlog.Controllers;

namespace SimpleBlog
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
			// this to point to a specific Posts controller
			string[] namespaces = new string[] {typeof(PostsController).Namespace};
			
			routes.MapRoute("TagWorkAround", "tag/{idAndSlug}", new { controller = "Posts", action = "Tag"}, namespaces);
			routes.MapRoute("Tag", "tag/{id}-{slug}", new { controller = "Posts", action = "Tag"}, namespaces);
			
			routes.MapRoute("PostWorkAround", "post/{idAndSlug}", new { controller = "Posts", action = "Show"}, namespaces);
			routes.MapRoute("Post", "post/{id}-{slug}", new { controller = "Posts", action = "Show"}, namespaces);
			
			routes.MapRoute("Login", "login", new {controller = "Auth", action = "Login"}, namespaces);
			routes.MapRoute("Logout", "logout", new {controller = "Auth", action = "Logout"}, namespaces);
			routes.MapRoute("Home", "", new {controller = "Posts", action = "Index"}, namespaces);
		}
	}
}