using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.API
{
	/// <summary>IOrionLogger 介面</summary>
	public interface IOrionLoggerFactory
	{
		/// <summary></summary>
		IOrionLogger Create();

		/// <summary></summary>
		IOrionLogger Create(Type type);

		/// <summary></summary>
		IOrionLogger Create(string name);
	}
}
