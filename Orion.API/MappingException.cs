using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;


namespace Orion.API
{


	/// <summary>Custom Exception</summary>
	[Serializable]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Exception))]
	[ComVisible(true)]
	public class MappingException : Exception
	{
		/// <summary>Custom Exception That Just One Parm</summary>
		public MappingException(string message) : base(message) { }

		/// <summary>Custom Exception That Just Two Parms</summary>
		public MappingException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>Custom Exception 須遵守嚴格的安全性稽核，以確保其可在安全的執行環境中使用</summary>
		[SecuritySafeCritical]
		protected MappingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}

}
