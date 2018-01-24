using Orion.API.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.API
{
	/// <summary>Model Mapping Config 類</summary>
	public class MappingConfig<From, To> where From : class where To : new()
	{

		private Action<From, To> _mappingProperties = (f, t) => { };
		private HashSet<string> _ignoreFrom = new HashSet<string>();
		private HashSet<string> _ignoreTo = new HashSet<string>();


		private List<PropertyInfo> _propTo = typeof(To)
			.GetProperties()
			.Where(x => x.CanWrite)
			.ToList();



		private string getTypeName(Type type)
		{
			var ntype = Nullable.GetUnderlyingType(type);
			if (ntype != null) { return ntype.Name + "?"; }

			if (type.IsGenericType)
			{
				string generic = type.GetGenericArguments()
					.Select(x => x.Name)
					.Aggregate((f, s) => f + ", " + s);

				return type.Name + "<" + generic + ">";
			}

			return type.Name;
		}




		private PropertyInfo getProperty(LambdaExpression lambdaExpr)
		{
			PropertyInfo prop = lambdaExpr.GetProperty();
			if (prop != null) { return prop; }
			throw new MappingException("無法取得 " + lambdaExpr + " 的 Property 名稱");			
		}





		/// <summary>設定 Mapping 的欄位對應</summary>
		public MappingConfig<From, To> Mapping<T>(Expression<Func<To, T>> toProp, Expression<Func<From, T>> fromProp)
		{
			PropertyInfo prop = getProperty(toProp);

			var getValue = fromProp.Compile();
			_mappingProperties += (f, t) => { prop.SetValue(t, getValue(f)); };

			IgnoreFrom(fromProp);
			IgnoreTo(toProp);

			return this;
		}




		/// <summary>將 from 需要 Ignore的加入清單，並取回 MappingConfig</summary>
		public MappingConfig<From, To> IgnoreFrom<T>(Expression<Func<From, T>> fromProp)
		{
			PropertyInfo prop = fromProp.GetProperty();
			if (prop != null) { _ignoreFrom.Add(prop.Name); }
			return this;
		}

		/// <summary>將 to 需要 Ignore的加入清單，並取回 MappingConfig</summary>
		public MappingConfig<From, To> IgnoreTo<T>(Expression<Func<To, T>> toProp)
		{
			PropertyInfo prop = getProperty(toProp);
			_ignoreTo.Add(prop.Name);
			return this;
		}

		/// <summary>將 from、to 需要 Ignore的加入清單，並取回 MappingConfig</summary>
		public MappingConfig<From, To> IgnoreBoth<T>(Expression<Func<From, T>> toProp)
		{
			PropertyInfo prop = getProperty(toProp);
			_ignoreFrom.Add(prop.Name);
			_ignoreTo.Add(prop.Name);
			return this;
		}




		/// <summary>取得 MappingProperties 的 Action</summary>
		public Action<From, To> Build()
		{
			var fromPropDict = typeof(From).GetProperties()
				.Where(x => x.CanRead)
				.ToDictionary(x => x.Name, x => x);


			var errorMsg = new List<string>();

			foreach (var prop in _propTo)
			{
				string name = prop.Name;
				if (_ignoreTo.Contains(name)) { continue; }
				if (prop.PropertyType.IsTypeOf(typeof(EntitySet<>))) { continue; }


				if (!fromPropDict.ContainsKey(name))
				{
					errorMsg.Add($"找不到 [ {name} ] 對應的 Property！");
					continue;
				}

				var fromProp = fromPropDict[name];

				if (fromProp.PropertyType != prop.PropertyType)
				{
					var msg = $"[ {name} ] 的型態無法從 {getTypeName(fromProp.PropertyType)} 到 {getTypeName(prop.PropertyType)}！";
					errorMsg.Add(msg);
					continue;
				}

				fromPropDict.Remove(name);
				_mappingProperties += (f, t) => { prop.SetValue(t, fromProp.GetValue(f)); };
			}


			/*檢查缺少對應的來源 Property */
			var lostMapping = fromPropDict
				.Where(x => !x.Value.PropertyType.IsTypeOf(typeof(EntitySet<>)))
				.Where(x => !_ignoreFrom.Contains(x.Key))
				.ToList(x => x.Key);

			if (lostMapping.Count > 0)
			{ errorMsg.Add($"來源 Property [ {string.Join(", ", lostMapping)} ] 缺少對應！"); }

			if (errorMsg.Count > 0)
			{
				throw new MappingException(string.Join("\n", errorMsg));
			}

			return _mappingProperties;
		}




	}



}
