using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.API.Extensions
{
	/// <summary>定義 IEnumerable 與泛型 IEnumerable 的 Extension</summary>
	public static class EnumerableExtensions
	{

		/// <summary>從 IEnumerable 建立 List</summary>
		public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToList();
		}

		/// <summary>從 IEnumerable 建立 List</summary>
		public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToArray();
		}



		/// <summary> 賦予 IEnumerable ForEach Method</summary>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration) 
			{ 
				action(item);
			}
		}

		/// <summary> 賦予 IEnumerable ForEach Method</summary>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			int i = 0;
			foreach (T item in enumeration)
			{
				action(item, i++);
			}
		}



		/// <summary> 賦予 IEnumerable Each Method</summary>
		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
				yield return item;
			}
		}

		/// <summary> 賦予 IEnumerable Each Method</summary>
		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			int i = 0;
			foreach (T item in enumeration)
			{
				action(item, i++);
				yield return item;
			}
		}


		/// <summary> 到 string.Join, 輸入 null 則回傳 null</summary>
		public static string JoinBy<T>(this IEnumerable<T> enumeration, string separator)
		{
			if(enumeration == null) { return null; }
			return string.Join(separator, enumeration);
		}



		/// <summary> 轉換到 HashSet</summary>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumeration)
		{
			return new HashSet<T>(enumeration);
		}



		/// <summary> OrderBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		/// <typeparam name="TSource">來源型態</typeparam>
		/// <typeparam name="TKey">選擇欄位型態</typeparam>
		/// <param name="source">Extension Method this</param>
		/// <param name="keySelector">選擇排序欄位的 Delegate</param>
		/// <param name="descending">正反排序</param>
		/// <returns>IOrderedEnumerable&lt;TSource&gt;</returns>
		/// <example>
		///	<code>
		///		var list = new List&lt;int&gt;(){2,5,1,3,7,4};
		///		IEnumerableExtensions.OrderBy&lt;int, int&gt;(list, x =&gt; x, true);
		///		list.OrderBy&lt;int, int&gt;(x =&gt; x, true);
		/// </code>
		/// </example>
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
		{
			return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
		}


		/// <summary> ThenBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
		{
			return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
		}


		/// <summary> OrderBy 擴充，可直接使用 Enum Field 排序，Field Name 必須與 Column Name 一致</summary>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Enum keySelector, bool descending)
		{
			return OrderBy(source, keySelector.ToString(), descending);
		}

		/// <summary> OrderBy 擴充</summary>
		/// <param name="source"></param>
		/// <param name="keySelector">field Name, propertyName</param>
		/// <param name="descending">true Asc; false Desc</param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string keySelector, bool descending)
		{
			Type modelType = typeof(TSource);
			var prop = modelType.GetProperty(keySelector);
			if (prop == null) { throw new ArgumentOutOfRangeException(keySelector, "不存在"); }


			Type propType = prop.PropertyType;
			var paramExpr = Expression.Parameter(modelType, "x");
			var propExpr = Expression.Property(paramExpr, prop);

			Type funcType = typeof(Func<,>).MakeGenericType(modelType, propType);
			MethodInfo lambdaMethod = Utils.LambdaMethod.MakeGenericMethod(funcType);
			MethodInfo orderByMethod = Utils.EnumerableOrderByMethod.MakeGenericMethod(modelType, propType);


			var keyExpression = (LambdaExpression)lambdaMethod.Invoke(null, new object[] { propExpr, new[] { paramExpr } });

			return (IOrderedEnumerable<TSource>)orderByMethod.Invoke(null, new object[] 
			{ 
				source, keyExpression.Compile(), descending 
			});
		}




		/// <summary>傳回最大的結果值；如果找不到則傳回預設值。</summary>
		public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source) where TSource : struct
		{
			return source.MaxOrDefault(x => x);
		}

		/// <summary>傳回最大的結果值；如果找不到則傳回預設值。</summary>
		public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.AsQueryable().MaxOrDefault(selector);
		}


		/// <summary>傳回最小的結果值；如果找不到則傳回預設值。</summary>
		public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source) where TSource : struct
		{
			return source.MinOrDefault(x => x);
		}

		/// <summary>傳回最小的結果值；如果找不到則傳回預設值。</summary>
		public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.AsQueryable().MinOrDefault(selector);
		}








		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static int SumOrDefault(this IEnumerable<int> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static long SumOrDefault(this IEnumerable<long> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static float SumOrDefault(this IEnumerable<float> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static double SumOrDefault(this IEnumerable<double> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static decimal SumOrDefault(this IEnumerable<decimal> source) { return source.SumOrDefault(x => x); }



		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static int SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static long SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static float SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static double SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static decimal SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}








		/// <summary>根據泛型參數轉型，若失敗則拋棄</summary>
		public static IEnumerable<TResult> Convert<TResult>(this IEnumerable source)
		{
			Type type = typeof(TResult);
			return Convert(source, type).Cast<TResult>();
		}



		/// <summary>根據 type 參數轉型，若失敗則拋棄</summary>
		public static IEnumerable<object> Convert(this IEnumerable source, Type type)
		{
			foreach (var value in source)
			{
				object result = OrionUtils.ConvertType(value, type);
				if (result == null) { continue; }

				yield return result;
			}
		}




		/// <summary>將資料依數量分區段</summary>
		public static IEnumerable<IEnumerable<T>> Bulk<T>(this IEnumerable<T> source, int blockSize)
		{
			if (source == null || !source.Any()) { return Enumerable.Empty<IEnumerable<T>>(); }

			return source
				.Select((value, i) => new { Value = value, Tag = i / blockSize })
				.GroupBy(x => x.Tag)
				.Select(g => g.Select(x => x.Value));
		}


		/// <summary>將資料依數量分區段</summary>
		public static IEnumerable<List<T>> BulkToList<T>(this IEnumerable<T> source, int blockSize)
		{
			return Bulk(source, blockSize).Select(x => x.ToList());
		}



		/// <summary>尋訪所有節點，廣度優先</summary>
		public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
		{
			var queue = new Queue<T>(items);
			while (queue.Count > 0)
			{
				T item = queue.Dequeue();
				yield return item;

				IEnumerable<T> childs = childSelector(item);
				if(childs == null) { continue; }
				foreach (var child in childs) { queue.Enqueue(child); }
			}
		}


	}
}
