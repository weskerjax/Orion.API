using Orion.API.Tests;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Orion.API.Extensions.Tests
{
	public class WhereHasExtensionsTests
	{
		private OrionApiDataContext _dc;

		public WhereHasExtensionsTests()
		{
			string mdfPath = Path.GetFullPath(@"..\..\OrionApi.mdf");
			string connection = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True";
			_dc = new OrionApiDataContext(connection);
		}


		public static IEnumerable<object[]> RunTest_Data
		{
			get
			{
				yield return new object[] { 
					new InvoiceIssueDomain { InvoicePrefix = "FF" }, " = @p0" 
				};
				yield return new object[] { 
					new InvoiceIssueDomain { InvoicePrefix = "" }, "Table(" 
				};
			}
		}

		[Theory]
		[MemberData("RunTest_Data")]
		public void RunTest(InvoiceIssueDomain domain, string expected)
		{
			var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix == domain.InvoicePrefix);
			var sql = query.ToString();

			Assert.Contains(expected, sql);	
		}


		[Fact]
		public void NullTest()
		{
			var domain = new InvoiceIssueDomain();

			var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix == domain.InvoicePrefix.ToString());
			var sql = query.ToString();

			Assert.Contains("Table(", sql);
		}


	}



	public class InvoiceIssueDomain
	{
		/// <summary>發票字軌</summary>
		public string InvoicePrefix { get; set; }

	}

}
