using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.API.Extensions
{
	/// <summary>Initialization Extension Utility</summary>
	internal static class Utils
	{

		public static readonly MethodInfo LambdaMethod;
		public static readonly MethodInfo EnumerableOrderByMethod;
		public static readonly MethodInfo QueryableOrderByMethod;

		static Utils()
		{
			LambdaMethod = typeof(Expression).GetMethods()
				.Where(m => m.IsGenericMethod)
				.Where(m => m.Name == "Lambda")
				.Where(m => m.GetParameters().Length == 2)
				.First();

			EnumerableOrderByMethod = typeof(EnumerableExtensions).GetMethods()
				.Where(m => m.IsGenericMethod)
				.Where(m => m.Name == "OrderBy")
				.Where(m => m.GetParameters().Length == 3)
				.Where(m => m.GetGenericArguments().Length == 2)
				.First();

			QueryableOrderByMethod = typeof(QueryableExtensions).GetMethods()
				.Where(m => m.IsGenericMethod)
				.Where(m => m.Name == "OrderBy")
				.Where(m => m.GetParameters().Length == 3)
				.Where(m => m.GetGenericArguments().Length == 2)
				.First();
		}

	 


	}
}