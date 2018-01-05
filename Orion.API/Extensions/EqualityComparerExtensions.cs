using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.API.Extensions
{
	/// <summary>對於需要實作 IEqualityComparer 的 Extension Method 進行的 Extension 定義</summary>
	public static class EqualityComparerExtensions
	{


		/// <summary> 擴充 Contains， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, TSource, bool> comparer)
		{
			return source.Contains(value, new LambdaComparer<TSource>(comparer));
		}

		/// <summary> 擴充 Distinct， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> comparer)
		{
			return source.Distinct(new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充  Except， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return first.Except(second, new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充 Intersect， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return first.Intersect(second, new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充 Union， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return first.Union(second, new LambdaComparer<TSource>(comparer));
		}



		/// <summary> 擴充 Contains， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item, Func<TSource, TSource, bool> comparer)
		{
			return source.Contains(item, new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充 Distinct， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source, Func<TSource, TSource, bool> comparer)
		{
			return source.Distinct(new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充 Except， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TSource, bool> comparer)
		{
			return source1.Except(source2, new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充 Intersect， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TSource, bool> comparer)
		{
			return source1.Intersect(source2, new LambdaComparer<TSource>(comparer));
		}
		/// <summary> 擴充 Union， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TSource, bool> comparer)
		{
			return source1.Union(source2, new LambdaComparer<TSource>(comparer));
		}



		/// <summary>
		/// 外面不需要知道這個類的存在，就算提供也不好用。
		/// </summary>
		internal class LambdaComparer<TSource> : IEqualityComparer<TSource>
		{

			private readonly Func<TSource, TSource, bool> _comparer;

			public LambdaComparer(Func<TSource, TSource, bool> comparer)
			{
				_comparer = comparer;
			}

			public bool Equals(TSource x, TSource y)
			{
				return _comparer(x, y);
			}

			public int GetHashCode(TSource obj)
			{
				return 0;
			}
		}






		/// <summary> 擴充 Contains， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static bool Contains<TSource, TProperty>(this IEnumerable<TSource> source, TSource value, Func<TSource, TProperty> getter)
		{
			return source.Contains(value, new LambdaComparer<TSource, TProperty>(getter));
		}

		/// <summary> 擴充 Distinct， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Distinct<TSource, TProperty>(this IEnumerable<TSource> source, Func<TSource, TProperty> getter)
		{
			return source.Distinct(new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充  Except， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Except<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> getter)
		{
			return first.Except(second, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充 Intersect， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Intersect<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> getter)
		{
			return first.Intersect(second, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充 Union， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IEnumerable<TSource> Union<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> getter)
		{
			return first.Union(second, new LambdaComparer<TSource, TProperty>(getter));
		}



		/// <summary> 擴充 Contains， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static bool Contains<TSource, TProperty>(this IQueryable<TSource> source, TSource item, Func<TSource, TProperty> getter)
		{
			return source.Contains(item, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充 Distinct， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Distinct<TSource, TProperty>(this IQueryable<TSource> source, Func<TSource, TProperty> getter)
		{
			return source.Distinct(new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充 Except， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Except<TSource, TProperty>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TProperty> getter)
		{
			return source1.Except(source2, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充 Intersect， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Intersect<TSource, TProperty>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TProperty> getter)
		{
			return source1.Intersect(source2, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary> 擴充 Union， 加入 IEqualityComparer 實作&lt;TSource&gt;</summary>
		public static IQueryable<TSource> Union<TSource, TProperty>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TProperty> getter)
		{
			return source1.Union(source2, new LambdaComparer<TSource, TProperty>(getter));
		}



		/// <summary>
		/// 外面不需要知道這個類的存在，就算提供也不好用。
		/// </summary>
		internal class LambdaComparer<TSource, TProperty> : IEqualityComparer<TSource>
		{

			private readonly Func<TSource, TProperty> _getter;

			public LambdaComparer(Func<TSource, TProperty> getter)
			{
				_getter = getter;
			}

			public bool Equals(TSource x, TSource y)
			{
				TProperty xValue = _getter(x);
				TProperty yValue = _getter(y);
				if (xValue == null || yValue == null) { return false; }

				return xValue.Equals(yValue);
			}

			public int GetHashCode(TSource obj)
			{
				return 0;
			}
		}

	}


}