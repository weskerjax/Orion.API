using Xunit;

namespace Orion.API.Tests
{
	public class OrionCertificateTests
	{
		[Fact]
		public void GenerateTest()
		{
			string certificate = OrionCertificate.Generate("Test");
			Assert.NotEmpty(certificate);
		}

	}
}