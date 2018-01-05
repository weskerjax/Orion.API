using System;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Orion.API.Extensions
{
	/// <summary>用於取得 Display、Description Attribute 設定名稱或描述 </summary>
	public static class DisplayNameExtensions
	{

		/// <summary>取得 Method | Property | Field  DisplayAttribute、DescriptionAttribute 中的字串</summary>
		public static string GetDisplayName(this MemberInfo info)
		{
			return OrionUtils.GetDisplayName(info);
		}


		/// <summary>取得 Method | Property | Field  DisplayAttribute 中的字串</summary>
		public static string GetDisplayName(this Enum enumValue)
		{
			return OrionUtils.GetEnumDisplayName(enumValue);
		}


	}

}