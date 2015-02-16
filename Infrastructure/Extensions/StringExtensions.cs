using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SimpleBlog.Infrastructure.Extensions
{
	static public class StringExtensions
	{
		public static string Slugify(this string input)
		{
			input = Regex.Replace(input, @"[~a-zA-Z0-9\s]", "");
			input = input.ToLower();
			input = Regex.Replace(input, @"\s+", "-");
			return input;
		}
	}
}