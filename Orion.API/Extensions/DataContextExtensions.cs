using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.API.Extensions
{
	/// <summary>定義 DatatContext 的 Extension</summary>
	public static class DataContextExtensions
	{


		/// <summary>判斷 Table 是否存在</summary>
		public static bool TableExists<TEntity>(this DataContext dc) where TEntity : class
		{
			try
			{
				bool t = dc.GetTable<TEntity>().Any();
				return true;
			}
			catch 
			{
				return false;
			}
		}

		private static string getTableName(Type tableType)
		{
			var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
			return tableAttr?.Name ?? tableType.Name;
		}


		/// <summary>解析 DataContext 產生的 Table Entity 中的 Attribute 屬性，來產生 CREATE TABLE 的 SQL 句</summary>
		public static string GetCreateSql<TEntity>(this Table<TEntity> table) where TEntity : class
		{
			Type tableType = typeof(TEntity);
			string tableName = getTableName(tableType);


			/* 處理的 ColumnAttribute 對象範例如下：
			 * [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MaterialCode", DbType="NVarChar(32) NOT NULL", CanBeNull=false)] 
			 * */

			var columnMeta = tableType.GetProperties().Select(info =>
			{
				var columnAttr = info.GetCustomAttribute<ColumnAttribute>();
				if (columnAttr == null) { return null; }

				string name = columnAttr.Name ?? info.Name;
				string dbType = (columnAttr.DbType ?? string.Empty).ToUpper();

				if (!columnAttr.CanBeNull && !dbType.Contains("NOT NULL")) { dbType += " NOT NULL"; }
				if (columnAttr.IsPrimaryKey && !dbType.Contains("PRIMARY KEY")) { dbType += " PRIMARY KEY"; }

				return $"[{name}] {dbType}";
			})
			.Where(meta => meta != null);

			return $"CREATE TABLE {tableName} ( {string.Join(",\n", columnMeta)} )";
		}


		/// <summary>根據 DataContext 的 Table Entity 中的 Attribute 屬性，來產生建立 DB Table</summary>
		public static void Create<TEntity>(this Table<TEntity> table) where TEntity : class
		{
			string createTableSql = GetCreateSql(table);
			table.Context.ExecuteCommand(createTableSql);
		}


		/// <summary>根據 DataContext 的 Table Entity 中的 Attribute 屬性，來 DROP TABLE</summary>
		public static void Drop<TEntity>(this Table<TEntity> table) where TEntity : class
		{
			string tableName = getTableName(typeof(TEntity));
			table.Context.ExecuteCommand($"DROP TABLE {tableName}");
		}


		/// <summary>根據 DataContext 的 Table Entity 中的 Attribute 屬性，來 TRUNCATE TABLE</summary>
		public static void Truncate<TEntity>(this Table<TEntity> table) where TEntity : class
		{
			string tableName = getTableName(typeof(TEntity));
			table.Context.ExecuteCommand($"TRUNCATE TABLE {tableName}");
		}


		/// <summary>根據 DataContext 的 Table Entity 中的 Attribute 屬性，來處理資料複製</summary>
		public static TableReplicator<From, To> CopyTo<From, To>(this Table<From> fromTable, Table<To> toTable) where From : class where To : class
		{
			return new TableReplicator<From, To>(fromTable.Context);
		}



		/// <summary>將查詢條件的所有實體置於 pending delete 狀態。</summary>
		public static void DeleteAllOnSubmit<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			table.DeleteAllOnSubmit(table.Where(predicate));
		}




		/*######################################################################*/


		private static Func<TEntity, TEntity, bool> getPrimaryKeyComparer<TEntity>()
		{
			Type type = typeof(TEntity);
			Expression binaryExpr = null;

			var aParamExpr = Expression.Parameter(type, "a");
			var bParamExpr = Expression.Parameter(type, "b");

			foreach (PropertyInfo prop in type.GetProperties())
			{
				var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
				if (columnAttr == null || !columnAttr.IsPrimaryKey) { continue; }

				var expr = Expression.Equal(
					Expression.Property(aParamExpr, prop), 
					Expression.Property(bParamExpr, prop)
				);

				if (binaryExpr != null) { expr = Expression.AndAlso(binaryExpr, expr); }
				binaryExpr = expr;
			}

			if (binaryExpr == null) { return null; }

			var lambda = Expression.Lambda<Func<TEntity, TEntity, bool>>(binaryExpr, aParamExpr, bParamExpr);
			return lambda.Compile();
		}



		private static Action<TEntity, TEntity> getPropertyCopyHandle<TEntity>(Func<string, bool> allowUpdateProperty)
		{
			Type type = typeof(TEntity);

			var ignore = new HashSet<string>();
			foreach (PropertyInfo prop in type.GetProperties())
			{
				if (!allowUpdateProperty(prop.Name)) { ignore.Add(prop.Name); continue; }

				var assocAttr = prop.GetCustomAttribute<AssociationAttribute>();
				if (assocAttr != null) { ignore.Add(assocAttr.ThisKey); }

				var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
				if (columnAttr != null && !columnAttr.IsPrimaryKey) { continue; }

				ignore.Add(prop.Name); 
			}


			Action<TEntity, TEntity> handle = ((from, to) => { });

			foreach (PropertyInfo prop in type.GetProperties())
			{
				if (ignore.Contains(prop.Name)) { continue; }

				handle += ((from, to) => 
				{
					prop.SetValue(to, prop.GetValue(from));
				});
			}

			return handle;
		}



		/// <summary>清單替換，比對差異完成新增、修改、刪除(Data Manipulation Language, DML)</summary>
		/// <code>
		///	_dc.WMS_CarrierMaterial.Replace(data.WMS_CarrierMaterial, list); 
		/// </code>
		public static void Replace<TEntity>(this Table<TEntity> entityTable, IEnumerable<TEntity> oldEntitys, IEnumerable<TEntity> newEntitys) where TEntity : class
		{
			Replace(entityTable, oldEntitys, newEntitys, name => !name.StartsWith("Create"));
		}


		/// <summary>清單替換，比對差異完成新增、修改、刪除(Data Manipulation Language, DML)</summary>
		/// <code>
		///	_dc.WMS_CarrierMaterial.Replace(data.WMS_CarrierMaterial, list, name =&gt; !name.StartsWith(&quot;Create&quot;)); 
		/// </code>
		public static void Replace<TEntity>(this Table<TEntity> entityTable, IEnumerable<TEntity> oldEntitys, IEnumerable<TEntity> newEntitys, Func<string, bool> allowUpdateProperty) where TEntity : class
		{
			Func<TEntity, TEntity, bool> pkComparer = getPrimaryKeyComparer<TEntity>();
			if (pkComparer == null) { throw new ArgumentException("TEntity 沒有指定 PrimaryKey"); }

			var entitySet = oldEntitys as EntitySet<TEntity>;
			var oldList = oldEntitys.ToList();
			var newList = newEntitys.ToList();


			/* Delete 的資料 */
			var deleteList = oldList.Except(newList, pkComparer);
			entityTable.DeleteAllOnSubmit(deleteList);


			/* 進行欄位複製處理 */
			Action<TEntity, TEntity> copyHandle = getPropertyCopyHandle<TEntity>(allowUpdateProperty);

			foreach (var newData in newList) 
			{
				var oldData = oldList.SingleOrDefault(x => pkComparer(x, newData));

				if (oldData != null)
				{
					/* Update */	
					copyHandle(newData, oldData); 
				}
				else if (entitySet != null)
				{
					/* Insert 到子表 */
					entitySet.Add(newData); 
				}
				else {
					/* Insert */
					entityTable.InsertOnSubmit(newData); 
				}
			}
		}






	}
}
