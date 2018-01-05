
using System;

namespace Orion.API.Models
{
	/// <summary>分頁參數</summary>
	public interface IPageParams
	{
		/// <summary></summary>
		int PageIndex { get; set; }

		/// <summary></summary>
		int PageSize { get; set; }

		/// <summary></summary>
		string OrderField { get; set; }

		/// <summary></summary>
		bool Descending { get; set; }
	}



	/// <summary>分頁參數</summary>
	public class PageParams<T> : IPageParams
	{

		/// <summary>不限制 PageSize</summary>
		public static PageParams<T> Unlimited() { return new PageParams<T> { PageSize = -1 }; }

		/// <summary></summary>
		public int PageIndex { get; set; }

		/// <summary></summary>
		public int PageSize { get; set; }

		/// <summary></summary>
		public T OrderField { get; set; }

		/// <summary></summary>
		public bool Descending { get; set; }


		string IPageParams.OrderField
		{
			get { return OrderField?.ToString(); }
			set { throw new NotImplementedException(); }
		}
	}
}
