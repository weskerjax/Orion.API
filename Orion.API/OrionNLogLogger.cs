using System;

namespace Orion.API
{

	/// <summary>nLog Tools</summary>
	public class OrionNLogLogger : IOrionLogger
	{
		private NLog.Logger _log;

		/// <summary></summary>
		public OrionNLogLogger() : this("Default") { }

		/// <summary></summary>
		public OrionNLogLogger(Type type) : this(type.FullName) { }

		/// <summary></summary>
		public OrionNLogLogger(string name)
		{
			_log = NLog.LogManager.GetLogger(name);
		}


		/// <summary>nlog Trace</summary>
		public void Trace(string message, Exception exception = null)
		{
			_log.Trace(exception, message); 
		}

		/// <summary>nlog Debug</summary>
		public void Debug(string message, Exception exception = null)
		{
			_log.Debug(exception, message); 
		}

		/// <summary>nlog Info</summary>
		public void Info(string message, Exception exception = null)
		{
			_log.Info(exception, message); 
		}

		/// <summary>nlog Warn</summary>
		public void Warn(string message, Exception exception = null)
		{
			_log.Warn(exception, message); 
		}

		/// <summary>nlog Error</summary>
		public void Error(string message, Exception exception = null)
		{
			_log.Error(exception, message); 
		}

		/// <summary>nlog Fatal</summary>
		public void Fatal(string message, Exception exception = null)
		{
			_log.Fatal(exception, message); 
		}

	}
	
}