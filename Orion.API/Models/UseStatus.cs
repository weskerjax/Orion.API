using System.ComponentModel.DataAnnotations;

namespace Orion.API.Models
{
	/// <summary>使用狀態</summary>
	public enum UseStatus
	{

		/// <summary>啟用</summary>
		[Display(Name = "啟用")]
		Enable,

		/// <summary>停用</summary>
		[Display(Name = "停用")]
		Disable,
	
	}

}
