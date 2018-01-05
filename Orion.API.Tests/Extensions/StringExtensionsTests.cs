using Orion.API.Tests;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Orion.API.Extensions.Tests
{
	public class StringExtensionsTests
	{
		public static IEnumerable<object[]> ToEnum_Test_Data()
		{
			yield return new object[] { "Create", JobStatus.Create };
			yield return new object[] { "Execute", JobStatus.Execute };
		}

		[Theory]
		[MemberData("ToEnum_Test_Data")]
		public void ToEnum_Test(string enumStr, JobStatus expected)
		{
			JobStatus result = enumStr.ToEnum<JobStatus>();
			Assert.Equal(expected, result);
		}


		public static IEnumerable<object[]> ToEnum_FailTest_Data()
		{
			yield return new object[] { "Creates" };
			yield return new object[] { "SSSS" };
			yield return new object[] { null };
		}

		[Theory]
		[MemberData("ToEnum_FailTest_Data")]
		public void ToEnum_FailTest(string enumStr)
		{
			Assert.ThrowsAny<Exception>(() => enumStr.ToEnum<JobStatus>());
		}




		public static IEnumerable<object[]> ToEnumOrDefault_Test_Data()
		{
			yield return new object[] { null, JobStatus.Create };
			yield return new object[] { "SSSS", JobStatus.Create };
			yield return new object[] { "Create", JobStatus.Create };
			yield return new object[] { "Execute", JobStatus.Execute };
		}

		[Theory]
		[MemberData("ToEnumOrDefault_Test_Data")]
		public void ToEnumOrDefault_Test(string enumStr, JobStatus expected)
		{
			JobStatus result = enumStr.ToEnum<JobStatus>(JobStatus.Create);
			Assert.Equal(expected, result);
		}





		//[Fact]
		//public void ToEnum_DbTest()
		//{
		//	var dc = new OrionApiDataContext();

		//	dc.InvoiceIssue.Select(x => new
		//	{
		//		UseStatus = x.DeliveryCustCode.ToEnum<UseStatus>()
		//	})
		//	.ToList();
		//}


	}



	public enum JobStatus
	{
		Create,
		Execute,
	}


}
