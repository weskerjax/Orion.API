
namespace Orion.API.Models
{
	/// <summary>查詢方式的定義</summary>
	public enum WhereOperator
	{
		/// <summary>In</summary>
		In,

		/// <summary>!In</summary>
		NotIn,
		
		/// <summary>=</summary>
		Equals,

		/// <summary>!=</summary>
		NotEquals,

		/// <summary>保含</summary>
		Contains,

		/// <summary>開頭等於</summary>
		StartsWith,

		/// <summary>結尾等於</summary>
		EndsWith,

		/// <summary>&lt;</summary>
		LessThan,

		/// <summary>&lt;=</summary>
		LessEquals,

		/// <summary>&gt;</summary>
		GreaterThan,

		/// <summary>&gt;=</summary>
		GreaterEquals,

		/// <summary>在之間</summary>
		Between,

		/// <summary>未定義</summary>
		Undefined,

		/// <summary>沒有值</summary>
		NotValue,

	}
}
