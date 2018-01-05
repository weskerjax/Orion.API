using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Orion.API.Extensions
{
	/// <summary>定義 IEnumerable&lt;TSource&gt; 的 Extension</summary>
	public static class WhereHasExtensions
	{

		private static bool hasQueryValue(LambdaExpression lambdaExpr)
		{
			var findList = lambdaExpr.FindByType<MemberExpression>();

			var memberExpr = findList.FirstOrDefault(x => x.Expression != lambdaExpr.Parameters[0]);
			if (memberExpr == null) { return true; }

			var value = Expression.Lambda(memberExpr).Compile().DynamicInvoke();
			return OrionUtils.HasValue(value);
		}





		/// <summary>判斷是否有值後進行 where 篩選</summary>
		public static IEnumerable<TSource> WhereHas<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (!hasQueryValue(predicate)) { return source; }
			return source.Where(predicate.Compile());
		}

		/// <summary>判斷是否有值後進行 where 篩選</summary>
		public static IEnumerable<TSource> WhereHas<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
		{
			if (!hasQueryValue(predicate)) { return source; }
			return source.Where(predicate.Compile());
		}



		/// <summary>判斷是否有值後進行 where 篩選</summary>
		public static IQueryable<TSource> WhereHas<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) 
		{
			if (!hasQueryValue(predicate)) { return source; }
			return source.Where(predicate); 
		}

		/// <summary>判斷是否有值後進行 where 篩選</summary>
		public static IQueryable<TSource> WhereHas<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
		{
			if (!hasQueryValue(predicate)) { return source; }
			return source.Where(predicate);
		}


	}
}
