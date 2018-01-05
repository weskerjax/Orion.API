using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Orion.API.Extensions
{
	/// <summary>定義 Dictionary 的 Extension</summary>
	public static class DictionaryExtensions
	{

		/// <summary>以 Key 取得 Dictionary 的 Value，找不到就將 Key 轉成 Value 的 Type 回傳</summary>
		public static V GetOrKey<K, V>(this Dictionary<K, V> dictionary, K key)
		{
			return dictionary.ContainsKey(key) ? dictionary[key] : OrionUtils.ConvertType<V>(key);
		}

		/// <summary>以 Key 取得 Dictionary 的 Value，找不到就回傳預設值</summary>
		public static V GetOrDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue = default(V))
		{
			return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
		}

	}

}
