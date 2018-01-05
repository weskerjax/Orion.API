using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orion.API.Models
{

	/// <summary>分頁 Interface</summary>
	public interface IPagination
	{
		/// <summary>資料清單</summary>
		IList List { get; }

		/// <summary>當前頁號</summary>
		int PageNumber { get; }

		/// <summary>每頁大小</summary>
		int PageSize { get; }

		/// <summary>總筆數</summary>
		int TotalItems { get; }

		/// <summary>當頁第一筆編號</summary>
		int FirstItem { get; }

		/// <summary>是否有下一頁</summary>
		bool HasNextPage { get; }

		/// <summary>是否有前一頁</summary>
		bool HasPreviousPage { get; }

		/// <summary>當頁最後一筆編號</summary>
		int LastItem { get; }

		/// <summary>總共頁數</summary>
		int TotalPages { get; }
	}


	/// <summary>分頁 Class</summary>
	[DataContract]
	public class Pagination<T> : IPagination
	{
		IList IPagination.List { get { return List; } }

		/// <summary>資料清單</summary>
		[DataMember]
		public List<T> List { get; set; }

		/// <summary>當前頁號</summary>
		[DataMember]
		public int PageNumber { get; set; }

		/// <summary>每頁大小</summary>
		[DataMember]
		public int PageSize { get; set; }

		/// <summary>總筆數</summary>
		[DataMember]
		public int TotalItems { get; set; }



		/// <summary>當頁第一筆編號</summary>
		public int FirstItem
		{
			get
			{
				return (((this.PageNumber - 1) * this.PageSize) + 1);
			}
		}

		/// <summary>是否有下一頁</summary>
		public bool HasNextPage
		{
			get
			{
				return (this.PageNumber < this.TotalPages);
			}
		}

		/// <summary>是否有前一頁</summary>
		public bool HasPreviousPage
		{
			get
			{
				return (this.PageNumber > 1);
			}
		}

		/// <summary>當頁最後一筆編號</summary>
		public int LastItem
		{
			get
			{
				return ((this.FirstItem + this.List.Count) - 1);
			}
		}

		/// <summary>總共頁數</summary>
		public int TotalPages
		{
			get
			{
				return (int)Math.Ceiling((double)(((double)this.TotalItems) / ((double)this.PageSize)));
			}
		}
		 

	}
}