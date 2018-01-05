using Orion.API.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Orion.API.Extensions
{
	/// <summary>定義 Model Mapping 的 Extension</summary>
	public static class MappingModelExtensions
	{

		private static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();


		private static Func<TSource, TResult> getMappingHandleWithCache<TSource, TResult>(Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			string cacheKey = string.Join("_", typeof(TSource).Name, typeof(TResult).Name, setting.Method.Name);

			/* 備忘： mappingHandle = _cache[cacheKey]; */
			object mappingHandle = _cache.GetOrAdd(cacheKey, key => getMappingHandle(setting));
			return mappingHandle as Func<TSource, TResult>;
		}


		private static Func<TSource, TResult> getMappingHandle<TSource, TResult>(Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			var cfg = setting(new MappingConfig<TSource, TResult>());
			Action<TSource, TResult> propertiesMapping = cfg.Build();

			return source =>
			{
				if (source == null) { return default(TResult); }

				var result = new TResult();
				propertiesMapping(source, result);
				return result;
			};
		}



		/// <summary>取回 mapping 後的 ModelList</summary>
		public static List<TResult> MappingModel<TSource, TResult>(this IEnumerable<TSource> source)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			return MappingModel<TSource, TResult>(source, cfg => cfg);
		}

		/// <summary>根據 Mapping Config，取回 mapping 後的 ModelList</summary>
		public static List<TResult> MappingModel<TSource, TResult>(this IEnumerable<TSource> source, Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			Func<TSource, TResult> mappingHandle = getMappingHandle(setting);
			return source.Select(mappingHandle).ToList();
		}




		/// <summary>取回 mapping 後的 ModelPagination</summary>
		public static Pagination<TResult> MappingModel<TSource, TResult>(this Pagination<TSource> source)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			return MappingModel<TSource, TResult>(source, cfg => cfg);
		}

		/// <summary>根據 Mapping Config，取回 mapping 後的 ModelPagination</summary>
		public static Pagination<TResult> MappingModel<TSource, TResult>(this Pagination<TSource> source, Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			Func<TSource, TResult> mappingHandle = getMappingHandle(setting);

			var pagination = new Pagination<TResult>
			{
				List = source.List.Select(mappingHandle).ToList(),
				PageNumber = source.PageNumber,
				PageSize = source.PageSize,
				TotalItems = source.TotalItems
			};

			return pagination;
		}




		/// <summary>取回 mapping 後的 Model</summary>
		public static TResult MappingModel<TSource, TResult>(this TSource source)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return default(TResult); }

			return MappingModel<TSource, TResult>(source, cfg => cfg);
		}

		/// <summary>根據 Mapping Config，取回 mapping 後的 Model</summary>
		public static TResult MappingModel<TSource, TResult>(this TSource source, Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return default(TResult); }

			Func<TSource, TResult> mappingHandle = getMappingHandle(setting);
			return mappingHandle(source);
		}





		/*###################################################################*/


		/// <summary>取回 mapping 後的 ModelList</summary>
		public static List<TResult> MappingModelWithCache<TSource, TResult>(this IEnumerable<TSource> source)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			return MappingModelWithCache<TSource, TResult>(source, cfg => cfg);
		}

		/// <summary>根據 Mapping Config，取回 mapping 後的 ModelList</summary>
		public static List<TResult> MappingModelWithCache<TSource, TResult>(this IEnumerable<TSource> source, Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			Func<TSource, TResult> mappingHandle = getMappingHandleWithCache(setting);
			return source.Select(mappingHandle).ToList();
		}




		/// <summary>取回 mapping 後的 ModelPagination</summary>
		public static Pagination<TResult> MappingModelWithCache<TSource, TResult>(this Pagination<TSource> source)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			return MappingModel<TSource, TResult>(source, cfg => cfg);
		}

		/// <summary>根據 Mapping Config，取回 mapping 後的 ModelPagination</summary>
		public static Pagination<TResult> MappingModelWithCache<TSource, TResult>(this Pagination<TSource> source, Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return null; }

			Func<TSource, TResult> mappingHandle = getMappingHandleWithCache(setting);

			var pagination = new Pagination<TResult>
			{
				List = source.List.Select(mappingHandle).ToList(),
				PageNumber = source.PageNumber,
				PageSize = source.PageSize,
				TotalItems = source.TotalItems
			};

			return pagination;
		}




		/// <summary>取回 mapping 後的 Model</summary>
		public static TResult MappingModelWithCache<TSource, TResult>(this TSource source)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return default(TResult); }

			return MappingModelWithCache<TSource, TResult>(source, cfg => cfg);
		}

		/// <summary>根據 Mapping Config，取回 mapping 後的 Model</summary>
		public static TResult MappingModelWithCache<TSource, TResult>(this TSource source, Func<MappingConfig<TSource, TResult>, MappingConfig<TSource, TResult>> setting)
			where TSource : class
			where TResult : new()
		{
			if (source == null) { return default(TResult); }

			Func<TSource, TResult> mappingHandle = getMappingHandleWithCache(setting);
			return mappingHandle(source);
		}





	}
}
