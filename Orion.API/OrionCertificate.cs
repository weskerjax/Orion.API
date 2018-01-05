using System;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Orion.API
{
	/// <summary>授權驗證</summary>
	public static class OrionCertificate
	{
		/// <summary>使用有啟用的網卡 MAC Address 來產生驗證碼</summary>
		public static string Generate(string productName)
		{
			string processorId = new ManagementClass("Win32_Processor")
				.GetInstances().Cast<ManagementObject>()
				.Select(x => "" + x["ProcessorId"]).First();

			string identify = $@"
				Product Name: {productName} 
				Machine Name: {Environment.MachineName}
				CPU Id: {processorId}
			";

			byte[] source = Encoding.Default.GetBytes(identify);
			byte[] crypto = new SHA256CryptoServiceProvider().ComputeHash(source);
			string result = Convert.ToBase64String(crypto);
			return result;
		}

		/// <summary>驗證授權</summary>
		public static bool Verify(string productName, string certificate)
		{
			return certificate == Generate(productName);
		}
	}
}
