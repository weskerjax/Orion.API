using System;


namespace Orion.API
{
	/// <summary>IOrionLogger 介面</summary>
	public interface IOrionLogger
	{
		/// <summary></summary>
		void Debug(string message, Exception exception = null);
		
		/// <summary></summary>
		void Error(string message, Exception exception = null);
		
		/// <summary></summary>
		void Fatal(string message, Exception exception = null);
		
		/// <summary></summary>
		void Info(string message, Exception exception = null);
		
		/// <summary></summary>
		void Trace(string message, Exception exception = null);
		
		/// <summary></summary>
		void Warn(string message, Exception exception = null);
	}


	/// <summary></summary>
	public enum OrionLogLevel
	{
		/// <summary></summary>
		Trace,

		/// <summary></summary>
		Debug,
		
		/// <summary></summary>
		Info,
		
		/// <summary></summary>
		Warn,
		
		/// <summary></summary>
		Error,
		
		/// <summary></summary>
		Fatal,
		
	}

}
