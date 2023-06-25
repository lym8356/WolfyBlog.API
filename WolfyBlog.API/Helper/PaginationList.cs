using System;
using Microsoft.EntityFrameworkCore;

namespace WolfyBlog.API.Helper
{
	public class PaginationList<T> : List<T>
	{
		public int TotalPages { get; private set; }
		public int TotalCount { get; private set; }
		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;

		public int CurrentPage { get; set; }
		public int PageSize { get; set; }

		public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			AddRange(items);
			TotalCount = totalCount;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
		}

		public static async Task<PaginationList<T>> CreateAsync(
			int currentPage, int pageSize, IQueryable<T> result)
		{
			var totalCount = await result.CountAsync();
			var skip = (currentPage - 1) * pageSize;
			result = result.Skip(skip);
			result = result.Take(pageSize);

			var items = await result.ToListAsync();

			return new PaginationList<T>(totalCount, currentPage, pageSize, items);
		}
	}
}

