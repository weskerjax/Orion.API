using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Orion.API.Models
{

	/// <summary></summary>
	public class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new()
	{
		private Func<ConfigurationElement, object> _keySelect = x => x;

		private readonly List<T> _list = new List<T>();


		/// <summary></summary>
		public ConfigurationElementCollection()
		{
			var keyProp = typeof(T).GetProperties().Where(p =>
			{
				var attr = p.GetCustomAttribute<ConfigurationPropertyAttribute>();
				return attr != null && attr.IsKey;
			}).FirstOrDefault();

			if (keyProp != null) { _keySelect = x => keyProp.GetValue(x); }
		}


		/// <summary></summary>
		protected override ConfigurationElement CreateNewElement()
		{
			var el = new T();
			_list.Add(el);
			return el;
		}

		/// <summary></summary>
		protected override object GetElementKey(ConfigurationElement target)
		{
			return _keySelect(target);
		}

		/// <summary></summary>
		public new IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}
	}

}
