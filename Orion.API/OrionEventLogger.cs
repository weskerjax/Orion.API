using System;

namespace Orion.API
{

	/// <summary>nLog Tools</summary>
	public class OrionEventLogger : IOrionLogger
	{
		/// <summary></summary>
		public event Action<OrionLogLevel, string, Exception> Event;

		/// <summary></summary>
		public void Trace(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Trace, message, exception);
		}

		/// <summary></summary>
		public void Debug(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Debug, message, exception);
		}

		/// <summary></summary>
		public void Info(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Info, message, exception);
		}

		/// <summary></summary>
		public void Warn(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Warn, message, exception);
		}

		/// <summary></summary>
		public void Error(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Error, message, exception);
		}

		/// <summary></summary>
		public void Fatal(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Fatal, message, exception);
		}

	}
	
}