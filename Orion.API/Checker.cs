using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Orion.API
{
	/// <summary>
	/// 資料檢查器
	/// </summary>
	public static class Checker
	{
		private static void throwException(string errorMessage, params object[] args)
		{
			if (Regex.IsMatch(errorMessage, @"{\d+}"))
			{
				errorMessage = string.Format(errorMessage, args);
			}
			throw new OrionException(errorMessage);
		}


		/// <summary>檢查有資料</summary>
		public static void Has(object value, string errorMessage) 
		{
			if (OrionUtils.HasValue(value)) { return; }
			throwException(errorMessage, value); 
		}

		/// <summary>檢查是否等於指定值</summary>
		public static void Is<T>(T value, T expected, string errorMessage)
		{
			if (expected.Equals(value)) { return; }
			throwException(errorMessage, value, expected);
		}


		/// <summary>檢查是否在指定清單內</summary>
		public static void In<T>(T? value, IEnumerable<T> expected, string errorMessage) where T : struct
		{
			if (value.HasValue && expected.Contains(value.Value)) { return; }
			throwException(errorMessage, value, expected);
		}


		/// <summary>檢查是否在指定清單內</summary>
		public static void In<T>(T value, IEnumerable<T> expected, string errorMessage)
		{
			if (expected.Contains(value)) { return; }
			throwException(errorMessage, value, expected);
		}


		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(int value, int min, string errorMessage) 
		{
			if (value >= min) { return; }
			throwException(errorMessage, value, min);
		}
		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(float value, float min, string errorMessage)
		{
			if (value >= min) { return; }
			throwException(errorMessage, value, min);
		}
		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(double value, double min, string errorMessage)
		{
			if (value >= min) { return; }
			throwException(errorMessage, value, min);
		}
		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(decimal value, decimal min, string errorMessage)
		{
			if (value >= min) { return; }
			throwException(errorMessage, value, min);
		}


		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(int? value, int min, string errorMessage)
		{
			if(value == null) { return; }
			Min(value.Value, min, errorMessage);
		}
		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(float? value, float min, string errorMessage)
		{
			if (value == null) { return; }
			Min(value.Value, min, errorMessage);
		}
		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(double? value, double min, string errorMessage)
		{
			if (value == null) { return; }
			Min(value.Value, min, errorMessage);
		}
		/// <summary>檢查value是否符合最小限制</summary>
		public static void Min(decimal? value, decimal min, string errorMessage)
		{
			if (value == null) { return; }
			Min(value.Value, min, errorMessage);
		}







		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(int value, int max, string errorMessage)
		{
			if (value <= max) { return; }
			throwException(errorMessage, value, max);
		}
		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(float value, float max, string errorMessage)
		{
			if (value <= max) { return; }
			throwException(errorMessage, value, max);
		}
		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(double value, double max, string errorMessage)
		{
			if (value <= max) { return; }
			throwException(errorMessage, value, max);
		}
		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(decimal value, decimal max, string errorMessage)
		{
			if (value <= max) { return; }
			throwException(errorMessage, value, max);
		}


		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(int? value, int max, string errorMessage)
		{
			if (value == null) { return; }
			Max(value.Value, max, errorMessage);
		}
		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(float? value, float max, string errorMessage)
		{
			if (value == null) { return; }
			Max(value.Value, max, errorMessage);
		}
		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(double? value, double max, string errorMessage)
		{
			if (value == null) { return; }
			Max(value.Value, max, errorMessage);
		}
		/// <summary>檢查value是否符合最大限制</summary>
		public static void Max(decimal? value, decimal max, string errorMessage)
		{
			if (value == null) { return; }
			Max(value.Value, max, errorMessage);
		}




		/// <summary>檢查數值範圍</summary>
		public static void Range(int value, int min, int max, string errorMessage)
		{
			if (value >= min && value <= max) { return; }
			throwException(errorMessage, value, min, max);
		}
		/// <summary>檢查數值範圍</summary>
		public static void Range(float value, float min, float max, string errorMessage)
		{
			if (value >= min && value <= max) { return; }
			throwException(errorMessage, value, min, max);
		}
		/// <summary>檢查數值範圍</summary>
		public static void Range(double value, double min, double max, string errorMessage)
		{
			if (value >= min && value <= max) { return; }
			throwException(errorMessage, value, min, max);
		}
		/// <summary>檢查數值範圍</summary>
		public static void Range(decimal value, decimal min, decimal max, string errorMessage)
		{
			if (value >= min && value <= max) { return; }
			throwException(errorMessage, value, min, max);
		}


		/// <summary>檢查數值範圍</summary>
		public static void Range(int? value, int min, int max, string errorMessage)
		{
			if (value == null) { return; }
			Range(value.Value, min, max, errorMessage);
		}
		/// <summary>檢查數值範圍</summary>
		public static void Range(float? value, float min, float max, string errorMessage)
		{
			if (value == null) { return; }
			Range(value.Value, min, max, errorMessage);
		}
		/// <summary>檢查數值範圍</summary>
		public static void Range(double? value, double min, double max, string errorMessage)
		{
			if (value == null) { return; }
			Range(value.Value, min, max, errorMessage);
		}
		/// <summary>檢查數值範圍</summary>
		public static void Range(decimal? value, decimal min, decimal max, string errorMessage)
		{
			if (value == null) { return; }
			Range(value.Value, min, max, errorMessage);
		}



	 
		 

		/// <summary>檢查最小長度</summary>
		public static void MinLength(string value, int minLength, string errorMessage) 
		{
			if (value == null) { return; }
			if (value.Length >= minLength) { return; }
			throwException(errorMessage, value, minLength);
		}
		/// <summary>檢查最小長度</summary>
		public static void MinLength<T>(IEnumerable<T> value, int minLength, string errorMessage)
		{
			if (value == null) { return; }
			if (value.Count() >= minLength) { return; }
			throwException(errorMessage, value, minLength);
		}


		/// <summary>檢查最大長度</summary>
		public static void MaxLength(string value, int maxLength, string errorMessage)
		{
			if (value == null) { return; }
			if (value.Length <= maxLength) { return; }
			throwException(errorMessage, value, maxLength);
		}

		/// <summary>檢查最大長度</summary>
		public static void MaxLength<T>(IEnumerable<T> value, int maxLength, string errorMessage)
		{
			if (value == null) { return; }
			if (value.Count() <= maxLength) { return; }
			throwException(errorMessage, value, maxLength);
		}





		/// <summary>檢查長度範圍</summary>
		public static void RangeLength(string value, int minLength, int maxLength, string errorMessage)
		{
			if (value == null) { return; }

			int length = value.Length;
			if (length >= minLength && length <= maxLength) { return; }
			throwException(errorMessage, value, minLength, maxLength);
		}
		/// <summary>檢查長度範圍</summary>
		public static void RangeLength<T>(IEnumerable<T> value, int minLength, int maxLength, string errorMessage)
		{
			if (value == null) { return; }

			int length = value.Count();
			if (length >= minLength && length <= maxLength) { return; }
			throwException(errorMessage, value, minLength, maxLength);
		}







		/// <summary>檢查是否在清單中</summary>
		public static void Contains(string value, string items, string errorMessage)
		{
			if (value == null) { return; }
			if (items.ToLower().Contains(value.ToLower())) { return; }
			throwException(errorMessage, value, items);
		}
		/// <summary>檢查是否在清單中</summary>
		public static void Contains(string value, IEnumerable<string> items, string errorMessage)
		{
			if (value == null) { return; }
			items = items.Select(x => x.ToLower());
			if (items.Contains(value.ToLower())) { return; }
			throwException(errorMessage, value, items);
		}
		/// <summary>檢查是否在清單中</summary>
		public static void Contains<T>(T value, IEnumerable<T> items, string errorMessage)
		{
			if (value == null) { return; }
			if (items.Contains(value)) { return; }
			throwException(errorMessage, value, items);
		}




		/// <summary>檢查是否符合 Pattern</summary>
		public static void Pattern(string value, string pattern, string errorMessage)
		{
			if (value == null) { return; }
			if (Regex.IsMatch(value, pattern)) { return; }
			throwException(errorMessage, value, pattern);
		}




		/// <summary>檢查是否符合狀態轉換規則</summary>
		public static void StatusRule<T, Allow>(T fromStatus, T toStatus, IDictionary<T, Allow> rule, string errorMessage) where Allow : IEnumerable<T>
		{
			var allow = Enumerable.Empty<T>();
			if (rule.ContainsKey(fromStatus)) { allow = rule[fromStatus]; }
			if (allow.Contains(toStatus)) { return; }

			string fromStatusStr = OrionUtils.GetEnumDisplayName(fromStatus) ?? fromStatus.ToString();
			string toStatusStr = OrionUtils.GetEnumDisplayName(toStatus) ?? toStatus.ToString();
			throwException(errorMessage, fromStatusStr, toStatusStr);
		}



		/// <summary>是否在清單中</summary>
		public static bool IsIn<T>(this T value, params T[] args)
		{
			return args.Contains(value);
		}

		/// <summary>是否不在清單中</summary>
		public static bool NotIn<T>(this T value, params T[] args)
		{
			return !args.Contains(value);
		}


	}
}
