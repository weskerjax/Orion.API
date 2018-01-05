using Orion.API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Orion.API.Extensions
{
	/// <summary>定義 IList 的 Extension</summary>
	public static class ListExtensions
	{


		/// <summary>將 List 賦予先進先出的功能，取回並刪除第一筆資料</summary>
		public static T Shift<T>(this IList<T> list)
		{
			if (list.Count == 0) { return default(T); }

			T data = list[0];
			list.RemoveAt(0);
			return data;
		}


		/// <summary>將 List 賦予取出的功能，取回並刪除資料</summary>
		public static T TakeOut<T>(this IList<T> list, Func<T, bool> predicate)
		{
			if (list.Count == 0) { return default(T); }
			T data = list.FirstOrDefault(predicate);
			if(data == null) { return data; }

			list.Remove(data);
			return data;
		}



	}
}
