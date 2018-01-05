using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Orion.API.Extensions
{
	/// <summary>定義 DataTable 的 Extension</summary>
	public static class DataTableExtensions
	{

		/// <summary>將 DataRow 與 POCO Mapping，回傳 IEnumerable of POCO</summary>
		public static IEnumerable<TModel> MappingTo<TModel>(this DataTable table) where TModel : new()
		{
			if (table == null) { throw new ArgumentNullException("table", "不可以為 Null"); }
			if (table.Rows.Count == 0) { return Enumerable.Empty<TModel>(); }

			Func<DataRow, TModel> mapper = getMapper<TModel>(table.Columns);
			return table.Rows.Cast<DataRow>().Select(mapper);
		}


		private static Func<DataRow, TModel> getMapper<TModel>(DataColumnCollection columns) where TModel : new()
		{
			Action<DataRow, TModel> mapping = (row, model) => { };

			foreach (var prop in typeof(TModel).GetProperties())
			{
				if (!columns.Contains(prop.Name)) { continue; }

				mapping += (row, model) => 
				{ 
					object value = OrionUtils.ConvertType(row[prop.Name], prop.PropertyType);
					if(value == null){ return; }

					prop.SetValue(model, value);
				};
			}

			return (row => 
			{
				var model = new TModel();
				mapping(row, model);
				return model;
			});
		}
	}

}
