
namespace Orion.API.Models
{
	/// <summary>WhereBuilder 的查詢參數</summary>
	public interface IWhereParams
	{
		/// <summary>設定欄位值與查詢條件</summary>
		IWhereParams SetValues<TValue>(string name, WhereOperator oper, TValue[] values);

		/// <summary>設定欄位查詢條件</summary>
		IWhereParams SetOperator(string name, WhereOperator oper);

		/// <summary>移除欄位值與查詢條件</summary>
		IWhereParams RemoveValues(string name);

		/// <summary>取得查詢條件</summary>
		WhereOperator GetOperator(string name);

		/// <summary>取得欄位數值清單</summary>
		object[] GetValues(string name);


	}
}
