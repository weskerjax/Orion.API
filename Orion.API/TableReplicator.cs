using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Orion.API.Extensions;

namespace Orion.API
{
	/// <summary>DataContext Table Replicator</summary>
	public class TableReplicator<From, To> where From : class where To : class
	{
		private DataContext _context;

		Dictionary<string, string> _columnMapping = new Dictionary<string, string>();
		Dictionary<string, string> _conditionMapping = new Dictionary<string, string>();


		/// <summary></summary>
		public TableReplicator(DataContext context)
		{
			_context = context;
		}



		/// <summary>設定複製資料的欄位</summary>
		public TableReplicator<From, To> Set<T>(Expression<Func<From, T>> fromProp, Expression<Func<To, T>> toProp)
		{
			string fromName = getColumnName(fromProp);
			string toName = getColumnName(toProp);

			_columnMapping[fromName] = toName;
			return this;
		}

		/// <summary>設定條件的欄位</summary>
		public TableReplicator<From, To> Condition<T>(Expression<Func<From, T>> fromProp, Expression<Func<To, T>> toProp)
		{
			string fromName = getColumnName(fromProp);
			string toName = getColumnName(toProp);

			_conditionMapping[fromName] = toName;
			return this;
		}



		/// <summary></summary>
		public void Execute()
		{
			if (_columnMapping.Count == 0) { throw new ArgumentException($"請指定複製的欄位"); }

			string fromTable = getTableName<From>();
			string toTable = getTableName<To>();

			if (_conditionMapping.Count == 0)
			{
				string insertColumn = _columnMapping.Select(kv => $"[{kv.Value}]").JoinBy(", ");
				string selectColumn = _columnMapping.Select(kv => $"[{kv.Key}]").JoinBy(", ");

				/*新增不存在的資料*/
				_context.ExecuteCommand($@"
					INSERT {toTable} ( {insertColumn} )
					SELECT {selectColumn} FROM  {fromTable} 
				");
			}
			else
			{
				string setColumn = _columnMapping.Select(kv => $"T.[{kv.Value}] = F.[{kv.Key}]").JoinBy(", ");
				string condColumn = _conditionMapping.Select(kv => $"T.[{kv.Value}] = F.[{kv.Key}]").JoinBy(" AND ");

				/*更新資料*/
				_context.ExecuteCommand($@"
					UPDATE {toTable} SET {setColumn} 
					FROM {toTable} T INNER JOIN {fromTable} F ON {condColumn}
				");

				string insertColumn = _columnMapping.Select(kv => $"[{kv.Value}]").JoinBy(", ");
				string selectColumn = _columnMapping.Select(kv => $"F.[{kv.Key}]").JoinBy(", ");

				/*新增不存在的資料*/
				_context.ExecuteCommand($@"
					INSERT {toTable} ( {insertColumn} )
					SELECT {selectColumn} FROM  {fromTable} F
					WHERE NOT EXISTS ( SELECT 1 FROM {toTable} T WHERE {condColumn} )
				");
			}
		}



		private string getTableName<Table>()
		{
			Type tableType = typeof(Table);
			var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
			return tableAttr?.Name ?? tableType.Name;
		}


		private string getColumnName<TEntity, TProp>(Expression<Func<TEntity, TProp>> propExpr)
		{
			PropertyInfo prop = propExpr.GetProperty();
			if (prop == null) { throw new ArgumentException($"{propExpr} 無法取得 Property"); }
			
			var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
			return columnAttr?.Name ?? prop.Name;
		}



	}
}
