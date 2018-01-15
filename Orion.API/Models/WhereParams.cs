using Orion.API.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Collections;
using System.Globalization;

namespace Orion.API.Models
{

	/// <summary>WhereBuilder 的查詢參數</summary>
	/// <typeparam name="TParams">POCO Model Type</typeparam>
	[DataContract]
	[Serializable]
	[DebuggerDisplay("Source = {Source}")]
	public class WhereParams<TParams> : IWhereParams
	{

		[DataMember]
		private Dictionary<string, WhereParamsPair> _source = new Dictionary<string, WhereParamsPair>();

		/// <summary></summary>
		public Dictionary<string, WhereParamsPair> Source {  get { return _source; } }



		private string getPropertyName(LambdaExpression lambdaExpr)
		{
			PropertyInfo prop = lambdaExpr.GetProperty();
			if (prop != null) { return prop.Name; }

			throw new Exception("無法取得 " + lambdaExpr + " 的 Property 名稱");
		}





		/*===========================================================*/

		/// <summary>設定欄位值與查詢條件</summary>
		IWhereParams IWhereParams.SetValues<TValue>(string name, WhereOperator oper, TValue[] values)
		{
			if(values == null) { values = new TValue[0]; }

			string[] strings = values.Convert<string>().ToArray();
			_source[name] = new WhereParamsPair(oper, strings);
			return this;
		}



		/// <summary>設定欄位值與查詢條件</summary>
		private WhereParams<TParams> setValues<TProperty>(LambdaExpression lambdaExpr, WhereOperator oper, params TProperty[] values)
		{
			if (values == null) { values = new TProperty[0]; }
			string[] strings = values.Convert<string>().ToArray();

			string name = getPropertyName(lambdaExpr);
			_source[name] = new WhereParamsPair(oper, strings);
			return this;
		}

		/// <summary>設定欄位值與查詢條件</summary>
		public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, IEnumerable<TProperty>>> column, WhereOperator oper, params TProperty[] values)
		{
			return setValues(column, oper, values);
		}
		/// <summary>設定欄位值與查詢條件</summary>
		public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, List<TProperty>>> column, WhereOperator oper, params TProperty[] values)
		{
			return setValues(column, oper, values);
		}
		/// <summary>設定欄位值與查詢條件</summary>
		public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, TProperty[]>> column, WhereOperator oper, params TProperty[] values)
		{
			return setValues(column, oper, values);
		}
		/// <summary>設定欄位值與查詢條件</summary>
		public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, TProperty>> column, WhereOperator oper, params TProperty[] values)
		{
			return setValues(column, oper, values);
		}



		/*===========================================================*/

		/// <summary>設定欄位值與查詢條件</summary>
		IWhereParams IWhereParams.SetOperator(string name, WhereOperator oper)
		{
			if (!_source.ContainsKey(name)) { return this; }

			_source[name].Operator = oper;
			return this;
		}

		/// <summary>設定欄位值與查詢條件</summary>
		public WhereParams<TParams> SetOperator<TProperty>(Expression<Func<TParams, TProperty>> column, WhereOperator oper)
		{
			string name = getPropertyName(column);
			if (!_source.ContainsKey(name)) { return this; }

			_source[name].Operator = oper;
			return this;
		}


		/*===========================================================*/

		/// <summary>移除欄位值與查詢條件</summary>
		IWhereParams IWhereParams.RemoveValues(string name)
		{
			_source.Remove(name);
			return this;
		}

		/// <summary>移除欄位值與查詢條件</summary>
		public WhereParams<TParams> RemoveValues<TProperty>(Expression<Func<TParams, TProperty>> column)
		{
			string name = getPropertyName(column);
			_source.Remove(name);
			return this;
		}





		/*===========================================================*/

		/// <summary>取得查詢條件</summary>
		WhereOperator IWhereParams.GetOperator(string name)
		{
			return _source.ContainsKey(name) ? _source[name].Operator : WhereOperator.NotValue;
		}

		/// <summary>取得查詢條件</summary>
		public WhereOperator GetOperator<TProperty>(Expression<Func<TParams, TProperty>> column)
		{
			string name = getPropertyName(column);
			return _source.ContainsKey(name) ? _source[name].Operator : WhereOperator.NotValue;
		}






		/*===========================================================*/


		/// <summary>取得欄位數值清單</summary>
		object[] IWhereParams.GetValues(string name)
		{
			if (!_source.ContainsKey(name)) { return new object[]{}; }

			Type type = typeof(TParams).GetProperty(name).PropertyType;
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			else if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
			{
				type = type.GenericTypeArguments.First();
			}

			return _source[name].Values.Select(x => OrionUtils.ConvertType(x, type)).ToArray();
		}


		private TProperty[] getValues<TProperty>(LambdaExpression lambdaExpr)
		{
			string name = getPropertyName(lambdaExpr);
			if (!_source.ContainsKey(name)) { return new TProperty[] { }; }
			return _source[name].Values.Select(x => OrionUtils.ConvertType<TProperty>(x)).ToArray();
		}



		/// <summary>取得欄位數值清單</summary>
		public TProperty[] GetValues<TProperty>(Expression<Func<TParams, IEnumerable<TProperty>>> column)
		{
			return getValues<TProperty>(column);
		}
		/// <summary>取得欄位數值清單</summary>
		public TProperty[] GetValues<TProperty>(Expression<Func<TParams, List<TProperty>>> column)
		{
			return getValues<TProperty>(column);
		}
		/// <summary>取得欄位數值清單</summary>
		public TProperty[] GetValues<TProperty>(Expression<Func<TParams, TProperty[]>> column)
		{
			return getValues<TProperty>(column);
		}
		/// <summary>取得欄位數值清單</summary>
		public TProperty[] GetValues<TProperty>(Expression<Func<TParams, TProperty>> column)
		{
			return getValues<TProperty>(column);
		}




		/*===========================================================*/

		/// <summary>設定欄位值與查詢條件</summary>
		public WhereParams<TParams> Assign(Expression<Func<TParams, bool>> conditionExpr)
		{
			PropertyInfo prop = conditionExpr.GetProperty();
			if (prop == null) { throw new ArgumentException("無法取得 " + conditionExpr + " 的 Property 名稱"); }

			WhereOperator Oper = parseExprOperator(conditionExpr.Body);

			string[] values = parseExprValues(conditionExpr, prop.PropertyType);
			if (values == null) { throw new ArgumentException($"無法取得 {conditionExpr} 的值"); }

			_source[prop.Name] = new WhereParamsPair(Oper, values);
			return this;
		}


		private WhereOperator parseExprOperator(Expression expr)
		{
			if (expr is UnaryExpression) /* Not 運算元 */
			{
				var unaryExpr = expr as UnaryExpression;
				if(unaryExpr.NodeType != ExpressionType.Not) { throw new ArgumentException($"不支援 {unaryExpr} 的 {unaryExpr.NodeType} 類型"); }

				WhereOperator oper = parseExprOperator(unaryExpr.Operand);
				switch (oper)
				{
					case WhereOperator.In: return WhereOperator.NotIn; 
					case WhereOperator.NotIn: return WhereOperator.In; 
					case WhereOperator.Equals: return WhereOperator.NotEquals; 
					case WhereOperator.NotEquals: return WhereOperator.Equals; 
					case WhereOperator.LessThan: return WhereOperator.GreaterEquals; 
					case WhereOperator.LessEquals: return WhereOperator.GreaterThan; 
					case WhereOperator.GreaterThan: return WhereOperator.LessEquals; 
					case WhereOperator.GreaterEquals: return WhereOperator.LessThan; 
					case WhereOperator.Contains:
					case WhereOperator.StartsWith:
					case WhereOperator.EndsWith:
					case WhereOperator.Between:
					default:
						throw new ArgumentException($"{oper} 不支援 Not 查詢");
				}
			}
			else if (expr is BinaryExpression)
			{
				var binaryExpr = expr as BinaryExpression;
				switch (binaryExpr.NodeType)
				{
					case ExpressionType.Equal: return WhereOperator.Equals; 
					case ExpressionType.NotEqual: return WhereOperator.NotEquals; 
					case ExpressionType.GreaterThan: return WhereOperator.GreaterThan; 
					case ExpressionType.GreaterThanOrEqual: return WhereOperator.GreaterEquals; 
					case ExpressionType.LessThan: return WhereOperator.LessThan; 
					case ExpressionType.LessThanOrEqual: return WhereOperator.LessEquals; 
					default: throw new ArgumentException($"不支援 {binaryExpr} 的 {binaryExpr.NodeType} 類型");
				}
			}
			else if (expr is MethodCallExpression)
			{
				var methodCallExpr = expr as MethodCallExpression;
				bool isStringMethod = (methodCallExpr.Method.DeclaringType == typeof(string));

				switch (methodCallExpr.Method.Name)
				{
					case nameof(string.Contains): return (isStringMethod ? WhereOperator.Contains : WhereOperator.In);
					case nameof(string.StartsWith): return WhereOperator.StartsWith;
					case nameof(string.EndsWith): return WhereOperator.EndsWith;
					case nameof(Checker.IsIn): return WhereOperator.In;
					case nameof(Checker.NotIn): return WhereOperator.NotIn;
					default: throw new ArgumentException($"不支援 {methodCallExpr} 的 {methodCallExpr.Method.Name} 方法");
				}
			}
			throw new ArgumentException($"無法解析 {expr} 的運算元");
		}



		private string[] parseExprValues(LambdaExpression expr, Type type)
		{
			var valueExpr = findValueExpr(expr.Parameters[0], expr.Body);
			if (valueExpr == null) { return null; }

			object value = Expression.Lambda(valueExpr).Compile().DynamicInvoke();
			bool isEnumerable = (value is IEnumerable && !(value is string));
			IEnumerable enumerable = isEnumerable ? value as IEnumerable : new object[] { value };

			string[] values = enumerable
				.Cast<object>()
				.Select(x => OrionUtils.ConvertType(x, type))
				.Select(x => OrionUtils.ConvertType<string>(x))
				.ToArray();

			return values;
		}


		private Expression findValueExpr(ParameterExpression paramExpr, Expression expr)
		{
			if (expr is BinaryExpression)
			{
				var binaryExpr = expr as BinaryExpression;

				return new[] { binaryExpr.Left, binaryExpr.Right, binaryExpr.Conversion }
					.Select(x => findValueExpr(paramExpr, x))
					.FirstOrDefault(x => x != null);
			}
			else if (expr is ConstantExpression)
			{
				return expr;
			}
			else if (expr is LambdaExpression)
			{
				var lambdaExpr = expr as LambdaExpression;
				return findValueExpr(paramExpr, lambdaExpr.Body);
			}
			else if (expr is MemberExpression)
			{
				var memberExpr = expr as MemberExpression;
				return memberExpr.Expression == paramExpr ? null : memberExpr;
			}
			else if (expr is MethodCallExpression)
			{
				var methodCallExpr = expr as MethodCallExpression;
				bool isStringMethod = (methodCallExpr.Method.DeclaringType == typeof(string));

				switch (methodCallExpr.Method.Name)
				{
					case nameof(string.Contains): 
					case nameof(string.StartsWith): 
					case nameof(string.EndsWith):
					case nameof(string.CompareTo):
						return methodCallExpr.Arguments.Concat(new[] { methodCallExpr.Object })
							.Select(x => findValueExpr(paramExpr, x))
							.FirstOrDefault(x => x != null);
					case nameof(Checker.IsIn): 
					case nameof(Checker.NotIn):
						return methodCallExpr.Arguments[1];
					default: 
						return methodCallExpr;
				}
			}
			else if (expr is UnaryExpression)
			{
				var unaryExpr = expr as UnaryExpression;
				return findValueExpr(paramExpr, unaryExpr.Operand);
			}

			return expr;
		}



	}




	/*##########################################################*/

	/// <summary></summary>
	public static class WhereParams 
	{
		/// <summary>根據物件建立 WhereParams</summary>
		public static WhereParams<TParams> CreateByObject<TParams>(TParams obj)
		{
			IWhereParams param = new WhereParams<TParams>();
			var props = typeof(TParams).GetProperties().Where(x => x.CanRead);

			foreach (var prop in props)
			{
				var value = prop.GetValue(obj);
				if (!OrionUtils.HasValue(value)) { continue; }

				param.SetValues(prop.Name, WhereOperator.Equals, new[] { value });
			}

			return (WhereParams<TParams>)param;
		}
		 
	}






	/*##########################################################*/

	/// <summary></summary>
	public class WhereParamsPair
	{
		/// <summary></summary>
		public WhereOperator Operator { get; set; }

		/// <summary></summary>
		public string[] Values { get; set; }

		/// <summary></summary>
		public WhereParamsPair() { }

		/// <summary></summary>
		public WhereParamsPair(WhereOperator oper, string[] values)
		{
			Operator = oper;
			Values = values;
		}
	}




}
