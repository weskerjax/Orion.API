using System;
using System.Text.RegularExpressions;

namespace Orion.API.Extensions
{

	/// <summary>定義 String 的 Extension</summary>
	public static class StringExtensions
	{

		/// <summary>檢查 string 是否有文字</summary>
		public static bool HasText(this String s)
		{
			return !string.IsNullOrWhiteSpace(s);
		}

		/// <summary>檢查 string 是否沒有文字</summary>
		public static bool NoText(this String s)
		{
			return string.IsNullOrWhiteSpace(s);
		}

		/// <summary>Regex IsMatch 忽略大小寫</summary>
		public static bool IsMatch(this String input, string pattern)
		{
			return IsMatch(input, pattern, RegexOptions.IgnoreCase);
		}


		/// <summary>Regex IsMatch</summary>
		public static bool IsMatch(this String input, string pattern, RegexOptions option)
		{
			if (!HasText(input) || !HasText(pattern)) { return false; }
			return Regex.IsMatch(input, pattern, option);
		}


		/// <summary>限制字串長度</summary>
		public static string LimitLength(this String input, int limit)
		{
			if (input == null || input.Length <= limit) { return input; }
			return input.Substring(0, limit);
		}


		/// <summary>將 string Trim 掉空白</summary>
		public static string NullableTrim(this String s)
		{
			return string.IsNullOrEmpty(s) ? s : s.Trim();
		}

		/// <summary>將 string Trim 掉指定字元</summary>
		public static string NullableTrim(this String s, params char[] trimChars)
		{
			return string.IsNullOrEmpty(s) ? s : s.Trim(trimChars);
		}




		/// <summary>將 string 忽略大小寫後比較是否相等</summary>
		public static bool EqualsIgnoreCase(this String s, String t)
		{
			return string.Equals(s, t, StringComparison.OrdinalIgnoreCase);
		}




		/// <summary>將 string 轉換為 Enum，若失敗則拋出 Exception</summary>
		public static TEnum ToEnum<TEnum>(this String enumStr)
		{
			try
			{
				return (TEnum)Enum.Parse(typeof(TEnum), enumStr);
			}
			catch (ArgumentException ex)
			{
				throw new ArgumentException("無法轉換 " + enumStr + " 到 Enum " + typeof(TEnum).Name, ex);
			}
		}

		/// <summary>將 string 轉換為 Enum，若失敗則回傳指定 Default Enum</summary>
		public static TEnum ToEnum<TEnum>(this String enumStr, TEnum defaultValue)
		{
			try
			{
				return ToEnum<TEnum>(enumStr);
			}
			catch
			{
				return defaultValue;
			}
		}


	}

}