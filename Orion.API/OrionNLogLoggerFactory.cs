using System;
using System.Collections.Concurrent;

namespace Orion.API
{
	/// <summary></summary>
	public class OrionNLogLoggerFactory : IOrionLoggerFactory
	{
		private static ConcurrentDictionary<string, IOrionLogger> _cache = new ConcurrentDictionary<string, IOrionLogger>();

		private IOrionLogger getInstance(string name) 
		{
			return _cache.GetOrAdd(name, x => new OrionNLogLogger(name)); 
		}


		/// <summary></summary>
		public IOrionLogger Create() { return getInstance("Default"); }

		/// <summary></summary>
		public IOrionLogger Create(Type type) { return getInstance(type.FullName); }

		/// <summary></summary>
		public IOrionLogger Create(string name) { return getInstance(name); }

	}
}
