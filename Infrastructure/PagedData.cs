using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBlog.Infrastructure
{
	public class PagedData<T> : IEnumerable<T>
	{
		IEnumerable<T> m_backend;

		public int TotalCount { get; private set; }
		public int Page { get; private set; }
		public int PerPage { get; private set; }
		public int TotalPages { get; private set; }

		public bool HasNextPage { get; private set; }
		public bool HasPrevPage { get; private set; }

		public int NextPage
		{
			get
			{
				if (HasNextPage == false)
					throw new InvalidOperationException();
				return Page + 1;
			}
		}
		public int PrevPage
		{
			get
			{
				if (HasPrevPage == false)
					throw new InvalidOperationException();
				return Page - 1;
			}
		}

		public PagedData(IEnumerable<T> currentItems, int totalCount, int page, int perPage)
		{
			m_backend = currentItems;
			TotalCount = totalCount;
			Page = page;
			PerPage = perPage;
			TotalPages = (int)Math.Ceiling((float)TotalCount / (float)PerPage);
			HasNextPage = Page < TotalPages;
			HasPrevPage = page > 1;
		}

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return m_backend.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_backend.GetEnumerator();
		}
		#endregion
	}
}