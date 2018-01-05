using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.API
{

	/// <summary>定義 Lambda Expression 的 Extension</summary>
	public static class LambdaUtils
	{


		private static void recursiveExpression<T, TExpr>(List<T> result, ReadOnlyCollection<TExpr> exprs)
			where T : Expression
			where TExpr : Expression
		{
			foreach (var expr in exprs)
			{
				recursiveExpression(result, expr);
			}
		}



		private static void recursiveExpression<T>(List<T> result, Expression expr)
			where T : Expression
		{
			if (expr == null) { return; }
			if (expr is T) { result.Add((T)expr); }


			if (expr is LambdaExpression)
			{
				var lambdaExpr = expr as LambdaExpression;
				recursiveExpression(result, lambdaExpr.Body);
				recursiveExpression(result, lambdaExpr.Parameters);
			}
			else if (expr is BinaryExpression)
			{
				var binaryExpr = expr as BinaryExpression;
				recursiveExpression(result, binaryExpr.Left);
				recursiveExpression(result, binaryExpr.Right);
				recursiveExpression(result, binaryExpr.Conversion);
			}
			else if (expr is BlockExpression)
			{
				var blockExpr = expr as BlockExpression;
				recursiveExpression(result, blockExpr.Result);
				recursiveExpression(result, blockExpr.Variables);
				recursiveExpression(result, blockExpr.Expressions);
			}
			else if (expr is ConditionalExpression)
			{
				var condExpr = expr as ConditionalExpression;
				recursiveExpression(result, condExpr.Test);
				recursiveExpression(result, condExpr.IfTrue);
				recursiveExpression(result, condExpr.IfFalse);
			}
			else if (expr is DynamicExpression)
			{
				var dynamicExpr = expr as DynamicExpression;
				recursiveExpression(result, dynamicExpr.Arguments);
			}
			else if (expr is GotoExpression)
			{
				var gotoExpr = expr as GotoExpression;
				recursiveExpression(result, gotoExpr.Value);
			}
			else if (expr is IndexExpression)
			{
				var indexExpr = expr as IndexExpression;
				recursiveExpression(result, indexExpr.Object);
				recursiveExpression(result, indexExpr.Arguments);
			}
			else if (expr is InvocationExpression)
			{
				var invocationExpr = expr as InvocationExpression;
				recursiveExpression(result, invocationExpr.Expression);
				recursiveExpression(result, invocationExpr.Arguments);
			}
			else if (expr is ListInitExpression)
			{
				var listInitExpr = expr as ListInitExpression;
				recursiveExpression(result, listInitExpr.NewExpression);

				foreach (ElementInit item in listInitExpr.Initializers)
				{ recursiveExpression(result, item.Arguments); }
			}
			else if (expr is LoopExpression)
			{
				var loopExpr = expr as LoopExpression;
				recursiveExpression(result, loopExpr.Body);
			}
			else if (expr is MemberExpression)
			{
				var memberExpr = expr as MemberExpression;
				recursiveExpression(result, memberExpr.Expression);
			}
			else if (expr is MemberInitExpression)
			{
				var memberInitExpr = expr as MemberInitExpression;
				recursiveExpression(result, memberInitExpr.NewExpression);
			}
			else if (expr is MethodCallExpression)
			{
				var methodCallExpr = expr as MethodCallExpression;
				recursiveExpression(result, methodCallExpr.Object);
				recursiveExpression(result, methodCallExpr.Arguments);
			}
			else if (expr is NewArrayExpression)
			{
				var newArrayExpr = expr as NewArrayExpression;
				recursiveExpression(result, newArrayExpr.Expressions);
			}
			else if (expr is NewExpression)
			{
				var newExpr = expr as NewExpression;
				recursiveExpression(result, newExpr.Arguments);
			}
			else if (expr is RuntimeVariablesExpression)
			{
				var runtimeExpr = expr as RuntimeVariablesExpression;
				recursiveExpression(result, runtimeExpr.Variables);
			}
			else if (expr is SwitchExpression)
			{
				var switchExpr = expr as SwitchExpression;
				recursiveExpression(result, switchExpr.DefaultBody);
				recursiveExpression(result, switchExpr.SwitchValue);
			}
			else if (expr is TryExpression)
			{
				var tryExpr = expr as TryExpression;
				recursiveExpression(result, tryExpr.Body);
				recursiveExpression(result, tryExpr.Fault);
				recursiveExpression(result, tryExpr.Finally);
			}
			else if (expr is TypeBinaryExpression)
			{
				var typeBinaryExpr = expr as TypeBinaryExpression;
				recursiveExpression(result, typeBinaryExpr.Expression);
			}
			else if (expr is UnaryExpression)
			{
				var unaryExpr = expr as UnaryExpression;
				recursiveExpression(result, unaryExpr.Operand);
			}

		}



		/// <summary>尋找 Lambda Expression tree 中指定類型</summary>
		public static List<T> FindByType<T>(Expression expr) where T : Expression 
		{
			var result = new List<T>();
			recursiveExpression<T>(result, expr); 
			return result;
		}


		/// <summary>尋找 Lambda Expression tree 中的 MemberInfo</summary>
		public static MemberInfo GetMember(LambdaExpression expr)
		{
			var paramType = expr.Parameters[0].Type;

			return FindByType<MemberExpression>(expr)
				.Where(x => x.Expression.Type == paramType)
				.Select(x => x.Member)
				.FirstOrDefault();
		}


		/// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
		public static PropertyInfo GetProperty(LambdaExpression expr)
		{
			var paramType = expr.Parameters[0].Type;

			return FindByType<MemberExpression>(expr)
				.Where(x => x.Expression.Type == paramType)
				.Select(x => x.Member)
				.OfType<PropertyInfo>()
				.FirstOrDefault();	
		}




		/*=========================================================*/


		/// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
		public static MethodInfo GetMethod<T1>(Expression<Action<T1>> expr) { return GetMethod((LambdaExpression)expr); }

		/// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
		public static MethodInfo GetMethod(Expression<Action> expr) { return GetMethod((LambdaExpression)expr); }

		/// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
		public static MethodInfo GetMethod(Expression expr)
		{
			return FindByType<MethodCallExpression>(expr)
				.Select(x => x.Method)
				.FirstOrDefault();
		}


		/// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
		public static MethodInfo GetGenericMethodDefinition<T1>(Expression<Action<T1>> expr) { return GetGenericMethodDefinition((LambdaExpression)expr); }

		/// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
		public static MethodInfo GetGenericMethodDefinition(Expression<Action> expr) { return GetGenericMethodDefinition((LambdaExpression)expr); }

		/// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
		public static MethodInfo GetGenericMethodDefinition(Expression expr)
		{
			MethodInfo method = GetMethod(expr);
			if (method == null) { return null; }

			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{ method = method.GetGenericMethodDefinition(); }

			return method;
		}


	}
}