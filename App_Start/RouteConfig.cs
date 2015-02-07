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
			routes.MapRoute("Login", "login", new {controller = "Auth", action = "Login"}, namespaces);
			routes.MapRoute("Home", "", new {controller = "Posts", action = "Index"}, namespaces);
		}
	}
}