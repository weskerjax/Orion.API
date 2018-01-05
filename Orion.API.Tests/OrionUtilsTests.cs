using System;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using Xunit;

namespace Orion.API.Tests
{
	public class OrionUtilsTests
	{

		public static IEnumerable<object[]> ConvertType_Test_Data()
		{
			yield return new object[] { Guid.NewGuid(), typeof(string) };
			yield return new object[] { "d5507446-c53d-4767-9e62-e7096951db1a", typeof(Guid) };

			yield return new object[] { Floor.F1, typeof(string) };
			yield return new object[] { "F1", typeof(Floor) };

			yield return new object[] { new TimeSpan(12,0,0), typeof(string) };
			yield return new object[] { "12:00:00", typeof(TimeSpan)};
		}

		[Theory]
		[MemberData("ConvertType_Test_Data")]
		public void ConvertType_Test(object value, Type type)
		{
			object result = OrionUtils.ConvertType(value, type);
			Assert.NotNull(result);
		}



		/*=======================================================*/
	 

		public static IEnumerable<object[]> ParseSolarDay_Test_Data()
		{
			yield return new object[] { 110131, new DateTime(2010,5, 11) };
			yield return new object[] { 116131, new DateTime(2016, 5, 10) };
		}

		[Theory]
		[MemberData("ParseSolarDay_Test_Data")]
		public void ParseSolarDay_Test(int value, DateTime? date)
		{
			DateTime? res = OrionUtils.ParseSolarDay(value);
			Assert.Equal(res, date);
		}



		/*=======================================================*/



		public static IEnumerable<object[]> ParseCnDate_Test_Data()
		{
			yield return new object[] { "104", null };
			yield return new object[] { "104.5.5", new DateTime(2015, 5, 5) };
			yield return new object[] { "105.02.05", new DateTime(2016, 2, 5) };
		}

		[Theory]
		[MemberData("ParseCnDate_Test_Data")]
		public void ParseCnDate_Test(string value, DateTime? date)
		{
			DateTime? res = OrionUtils.ParseCnDate(value);
			Assert.Equal(res, date);
		}



		/*=======================================================*/

		[Theory]
		[InlineData(null, 0)]
		[InlineData("", 0)]
		[InlineData("1, 2, 3", 3)]
		public void ToIdsList_Test(string value, int length)
		{
			var list = OrionUtils.ToIdsList<string>(value);
			Assert.Equal(list.Count, length);
		}


		[Theory]
		[InlineData(null, 0)]
		[InlineData("", 0)]
		[InlineData("1,2,3", 3)]
		[InlineData("1,2,3,3,3", 3)]
		[InlineData("1,2,3,sss,ffff", 3)]
		public void ToIdsList_IntTest(string value, int length)
		{
			var list = OrionUtils.ToIdsList<int>(value);
			Assert.Equal(list.Count, length);
		}

		[Theory]
		[InlineData(null, 0)]
		[InlineData("", 0)]
		[InlineData("F1,F2,F3", 3)]
		[InlineData("F1,F2,F3,F3,F3", 3)]
		[InlineData("F1,F2,F3,sss,ffff", 3)]
		public void ToIdsList_EnumTest(string value, int length)
		{
			var list = OrionUtils.ToIdsList<Floor>(value);
			Assert.Equal(list.Count, length);
		}




		/*=======================================================*/

		[Fact]
		public void ToIdsString_Test()
		{
			Assert.Equal(OrionUtils.ToIdsString((string[])null), "");
			Assert.Equal(OrionUtils.ToIdsString(new string[] { }), "");
			Assert.Equal(OrionUtils.ToIdsString(new string[] { "1", "2", "2" }), "1,2");
			Assert.Equal(OrionUtils.ToIdsString(new int[] { }), "");
			Assert.Equal(OrionUtils.ToIdsString(new int[] { 1, 2, 2 }), "1,2");
		}







	}



	public class IssueDomain
	{
		public decimal Sum { get; set; }
		public string InvoicePrefix { get; set; }
		public UseStatus UseStatus { get; set; }
	}



	public enum Floor
	{
		None,
		F1,
		F2,
		F3,
		F4,
	}


}
