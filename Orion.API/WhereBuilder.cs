using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Orion.API.Extensions;

namespace Orion.API
{
	/// <summary>WhereBuild Tools</summary>
	public class WhereBuilder<TModel, TParams> 
	{

		private static MethodInfo _containsMethod = LambdaUtils
			.GetGenericMethodDefinition<IEnumerable<int>>(list => list.Contains(1));

		private static MethodInfo _anyMethod = LambdaUtils
			.GetGenericMethodDefinition<IEnumerable<int>>(list => list.Any(x => true));


		private IQueryable<TModel> _query;
		private WhereParams<TParams> _param;


		/// <summary></summary>
		public WhereBuilder(IQueryable<TModel> query, WhereParams<TParams> param)
		{
			_query = query;
			_param = param;
		}




		/*=====================================================================*/

		private string getPropertyName(LambdaExpression lambdaExpr)
		{
			PropertyInfo prop = lambdaExpr.GetProperty();
			if (prop != null) { return prop.Name; }

			throw new Exception("無法取得 " + lambdaExpr + " 的 Property 名稱");
		}




		/*=====================================================================*/

		private Expression getStringCondition(Expression parameter, WhereOperator oper, string[] values)
		{
			Type type = typeof(string);
			var containsMethod = _containsMethod.MakeGenericMethod(type);
			var compareMethod = LambdaUtils.GetMethod<string>(x => x.CompareTo(""));
			var zeroExpr = Expression.Constant(0);
			var valueExpr = Expression.Constant(values.FirstOrDefault());
			var valuesExpr = Expression.Constant(values);
			var compareExpr = Expression.Call(parameter, compareMethod, valueExpr);
			
			switch (oper)
			{
				case WhereOperator.Contains: /* x.Contains(value) */
					var stringContains = LambdaUtils.GetMethod<string>(x => x.Contains(""));
					return Expression.Call(parameter, stringContains, valueExpr);
				case WhereOperator.StartsWith: /* x.StartsWith(value) */
					var stringStartsWith = LambdaUtils.GetMethod<string>(x => x.StartsWith(""));
					return Expression.Call(parameter, stringStartsWith, valueExpr);
				case WhereOperator.EndsWith: /* x.EndsWith(value) */
					var stringEndsWith = LambdaUtils.GetMethod<string>(x => x.EndsWith(""));
					return Expression.Call(parameter, stringEndsWith, valueExpr);
				case WhereOperator.Equals: /* x == value */
					return Expression.Equal(parameter, valueExpr);
				case WhereOperator.NotEquals: /* x != value */
					return Expression.NotEqual(parameter, valueExpr);

				case WhereOperator.LessThan: /* x < value */
					return Expression.LessThan(compareExpr, Expression.Constant(0));
				case WhereOperator.LessEquals: /* x <= value */
					return Expression.LessThanOrEqual(compareExpr, Expression.Constant(0));
				case WhereOperator.GreaterThan: /* x > value */
					return Expression.GreaterThan(compareExpr, Expression.Constant(0));
				case WhereOperator.GreaterEquals: /* x >= value */
					return Expression.GreaterThanOrEqual(compareExpr, Expression.Constant(0));

				case WhereOperator.In: /* values.Contains(x) */
					return Expression.Call(containsMethod, valuesExpr, parameter);
				case WhereOperator.NotIn: /* !values.Contains(x) */
					return Expression.Not(Expression.Call(containsMethod, valuesExpr, parameter));
			}

			return null;
		}



		private Expression getValueCondition<T>(Expression parameter, WhereOperator oper, T[] values)
		{
			Type type = typeof(T);
			var tempExpr = Expression.Constant(values.FirstOrDefault());
			var valueExpr = Expression.Convert(tempExpr, type);
			var valuesExpr = Expression.Constant(values);
			var containsMethod = _containsMethod.MakeGenericMethod(type);

			switch (oper)
			{
				case WhereOperator.Equals: /* x == value */
					return Expression.Equal(parameter, valueExpr);
				case WhereOperator.NotEquals: /* x != value */
					return Expression.NotEqual(parameter, valueExpr);
				case WhereOperator.LessThan: /* x < value */
					return Expression.LessThan(parameter, valueExpr);
				case WhereOperator.LessEquals: /* x <= value */
					return Expression.LessThanOrEqual(parameter, valueExpr);
				case WhereOperator.GreaterThan: /* x > value */
					return Expression.GreaterThan(parameter, valueExpr);
				case WhereOperator.GreaterEquals: /* x >= value */
					return Expression.GreaterThanOrEqual(parameter, valueExpr);
				case WhereOperator.In: /* values.Contains(x) */
					return Expression.Call(containsMethod, valuesExpr, parameter);
				case WhereOperator.NotIn: /* !values.Contains(x) */
					return Expression.Not(Expression.Call(containsMethod, valuesExpr, parameter));
				case WhereOperator.Between: /* x >= values[0] && x <= values[1] */
					var value1Expr = Expression.Convert(Expression.Constant(values[1]), type);
					return Expression.AndAlso(
						Expression.GreaterThanOrEqual(parameter, valueExpr),
						Expression.LessThanOrEqual(parameter, value1Expr)
					);
			}

			return null;
		}



		private Expression getCondition<T>(Expression parameter, WhereOperator oper, T[] values)
		{
			if (_param == null) { return null; }
			if (values == null || values.Length == 0) { return null; }

			var strValues = values as string[];
			if (strValues != null)
			{
				/*字串類型*/
				return getStringCondition(parameter, oper, strValues);
			}
			else
			{
				/*數值或日期類型*/
				return getValueCondition(parameter, oper, values);
			}
		}





		private void bindEnumerable<T>(Expression<Func<TModel, IEnumerable<T>>> columnSelector, WhereOperator oper, T[] values)
		{
			Type type = typeof(T);
			var parameter = Expression.Parameter(type);

			Expression condition = getCondition(parameter, oper, values);
			if (condition == null) { return; }

			/* y => y == deliveryNum 
			 * x => x.Any(y => y == deliveryNum) 
			 * */
			MethodInfo anyMethod = _anyMethod.MakeGenericMethod(type);
			var conditionExpr = Expression.Lambda<Func<T, bool>>(condition, parameter);
			var invokedExpr = Expression.Call(anyMethod, columnSelector.Body, conditionExpr);
			var whereExpr = Expression.Lambda<Func<TModel, bool>>(invokedExpr, columnSelector.Parameters);

			_query = _query.Where(whereExpr);
		}






		/*=====================================================================*/


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind<T>(Expression<Func<TParams, IEnumerable<T>>> find, Expression<Func<TModel, IEnumerable<T>>> columnSelector)
		{
			if (_param == null) { return this; }
			T[] values = _param.GetValues(find);
			WhereOperator oper = _param.GetOperator(find);

			bindEnumerable(columnSelector, oper, values);
			return this;
		}


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind<T>(Expression<Func<TParams, T>> find, Expression<Func<TModel, IEnumerable<T>>> columnSelector)
		{
			if (_param == null) { return this; }
			T[] values = _param.GetValues(find);
			WhereOperator oper = _param.GetOperator(find);

			bindEnumerable(columnSelector, oper, values);
			return this;
		}


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind<T>(Expression<Func<TParams, T>> find, Expression<Func<TModel, T>> columnSelector)
		{
			if (_param == null) { return this; }
			T[] values = _param.GetValues(find);
			WhereOperator oper = _param.GetOperator(find);

			Expression condition = getCondition(columnSelector.Body, oper, values);
			if (condition == null) { return this; }

			/* x => x => x.name == find.CompanyCode
			 * */
			var whereExpr = Expression.Lambda<Func<TModel, bool>>(condition, columnSelector.Parameters);

			_query = _query.Where(whereExpr);
			return this;
		}



		/// <summary></summary>
		public IQueryable<TModel> Build()
		{
			return _query;
		}


	}
}
