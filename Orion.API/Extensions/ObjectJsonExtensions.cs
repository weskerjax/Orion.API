using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web;

namespace Orion.API.Extensions
{
	/// <summary>定義 Object 的 Extension</summary>
	public static class ObjectJsonExtensions
	{
		static IList<JsonConverter> DefaultConverters = new List<JsonConverter>()
		{
			new StringEnumConverter(), 
			new IsoDateTimeConverter(),
			//new StringEnumConverter() { CamelCaseText = true },
		};


		/// <summary>將object 轉換為 Json</summary>
		public static string ToJson(this object obj) 
		{
			var settings = new JsonSerializerSettings()
			{
				Converters = DefaultConverters,
				//ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};

			return JsonConvert.SerializeObject(obj, (Type)null, settings);
		}


		/// <summary>將object 轉換為 JsonRaw</summary>
		public static IHtmlString ToJsonRaw(this object obj)
		{
			return new HtmlString(ToJson(obj));
		}


		/// <summary>將 json string 轉換為 Object</summary>
		public static TObject JsonToObject<TObject>(this String jsonStr)
		{
			if (jsonStr == null) { return default(TObject); }
			return JsonConvert.DeserializeObject<TObject>(jsonStr);
		}



	}
}