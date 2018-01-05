
namespace Orion.API
{
	/// <summary>IPasswordHandle 介面</summary>
	public interface IPasswordHandle
	{
		/// <summary></summary>
		bool Validate(string password);
		
		/// <summary></summary>
		string Encrypt(string password);

	}
}
