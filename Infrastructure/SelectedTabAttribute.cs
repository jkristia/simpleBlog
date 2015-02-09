using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog.Infrastructure
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class SelectedTabAttribute : ActionFilterAttribute
	{
		public string SelectedTab { get; private set; }
		public SelectedTabAttribute(string selectedTab)
		{
			SelectedTab = selectedTab;
		}
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			filterContext.Controller.ViewBag.SelectedTab = SelectedTab;
		}
	}
}