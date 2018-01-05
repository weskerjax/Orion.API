using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Orion.API
{
	/// <summary>SHA256 Tools</summary>
	public class PasswordSHA256Handle : IPasswordHandle
	{
		/// <summary>驗證 SHA256 字串是否由 a-z 加 A-Z 加 0-9 組成</summary>
		public bool Validate(string password)
		{
			if (password.Length < 6) { return false; }
			if (!Regex.IsMatch(password, @"[a-z]")) { return false; }
			if (!Regex.IsMatch(password, @"[A-Z]")) { return false; }
			if (!Regex.IsMatch(password, @"[0-9]")) { return false; }

			return true;
		}

		/// <summary>將 string to SHA256 to Base64 </summary>
		public string Encrypt(string password)
		{
			SHA256 sha256 = new SHA256CryptoServiceProvider();
			byte[] source = Encoding.Default.GetBytes(password);
			byte[] crypto = sha256.ComputeHash(source);
			string result = Convert.ToBase64String(crypto);
			return result;
		}
	}
}
