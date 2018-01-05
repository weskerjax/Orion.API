using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.API.Extensions
{
	/// <summary>定義 Pagination&lt;TSource&gt; 的 Extension</summary>
	public static class PaginationExtensions
	{

		/// <summary>預設換頁大小</summary>
		public static int DefaultPageSize { get; set; } = 20;


		/// <summary>取得換頁泛型物件</summary>
		public static Pagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
		{
			IQueryable<T> query = source.AsQueryable<T>();

			var result = new Pagination<T>
			{
				PageNumber = Math.Max(1, pageNumber),
				PageSize = pageSize,
			};

			if (pageSize == -1)
			{
				result.List = query.ToList();
				result.TotalItems = result.List.Count;
				return result;
			}

			result.TotalItems = query.Count();
			if (result.TotalItems == 0)
			{
				result.List = new List<T>();
				return result;
			}

			if (result.PageSize <= 0) { result.PageSize = DefaultPageSize; }
			int totalPages = (int)Math.Ceiling(((double)result.TotalItems) / result.PageSize);
			result.PageNumber = Math.Min(result.PageNumber, totalPages);

			int skip = (result.PageNumber - 1) * result.PageSize;
			result.List = query.Skip(skip).Take(result.PageSize).ToList();

			return result;
		}



		/// <summary>將 Pagination 的 List Member Projection 後，再次傳回 Pagination</summary>
		public static Pagination<TResult> As<TSource, TResult>(this Pagination<TSource> src, Func<TSource, TResult> selector)
		{
			var pagination = new Pagination<TResult>
			{
				List = src.List.Select(selector).ToList(),
				PageNumber = src.PageNumber,
				PageSize = src.PageSize,
				TotalItems = src.TotalItems
			};

			return pagination;
		}

		 
	}
}
