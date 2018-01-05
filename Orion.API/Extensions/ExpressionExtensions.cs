using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.API.Extensions
{

	/// <summary>定義 Lambda Expression 的 Extension</summary>
	public static class ExpressionExtensions
	{ 

		/// <summary>尋找 Lambda Expression tree 中指定類型</summary>
		public static List<T> FindByType<T>(this Expression expr) where T : Expression 
		{
			return LambdaUtils.FindByType<T>(expr);
		}

		/// <summary>尋找 Lambda Expression tree 中的 MemberInfo</summary>
		public static MemberInfo GetMember(this LambdaExpression expr)
		{
			return LambdaUtils.GetMember(expr);
		}

		/// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
		public static PropertyInfo GetProperty(this LambdaExpression expr)
		{
			return LambdaUtils.GetProperty(expr);
		}


		/// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
		public static MethodInfo GetMethod(this Expression expr)
		{
			return LambdaUtils.GetMethod(expr);
		}

		/// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
		public static MethodInfo GetGenericMethodDefinition(this Expression expr)
		{
			return LambdaUtils.GetGenericMethodDefinition(expr);
		}


	}
}