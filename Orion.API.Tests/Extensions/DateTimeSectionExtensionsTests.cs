using System.Collections.Generic;
using Orion.API.Models;
using Xunit;

namespace Orion.API.Extensions.Tests
{
	public class DateTimeSectionExtensionsTests
	{

		private static DateTimeSection section(string start, string end)
		{
			return new DateTimeSection(start, end);
		}


		private static DateTimeSection[] _sourceList = new[]
		{
			section("2017-08-01 00:00:00", "2017-08-01 08:00:00"),
			section("2017-08-01 08:00:00", "2017-08-01 20:00:00"),
			section("2017-08-01 20:00:00", "2017-08-02 00:00:00"),
			section("2017-08-02 00:00:00", "2017-08-02 07:00:00"),
			section("2017-08-02 07:00:00", "2017-08-02 12:00:00"),
			section("2017-08-02 18:00:00", "2017-08-03 00:00:00"),
			section("2017-08-03 00:00:00", "2017-08-03 07:00:00"),
			section("2017-08-03 07:00:00", "2017-08-03 12:00:00"),
			section("2017-08-03 12:00:00", "2017-08-03 18:00:00"),
			section("2017-08-03 18:00:00", "2017-08-04 00:00:00"),
		};


		public static IEnumerable<object[]> IntersectSectionTestData()
		{
			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-08-01 06:10:00", "2017-08-01 10:10:00"),
				},
				new[]
				{
					section("2017-08-01 06:10:00", "2017-08-01 08:00:00"),
					section("2017-08-01 08:00:00", "2017-08-01 10:10:00"),
				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-08-01 06:10:00", "2017-08-03 18:10:00"),
				},
				new[]
				{
					section("2017-08-01 06:10:00", "2017-08-01 08:00:00"),
					section("2017-08-01 08:00:00", "2017-08-01 20:00:00"),
					section("2017-08-01 20:00:00", "2017-08-02 00:00:00"),
					section("2017-08-02 00:00:00", "2017-08-02 07:00:00"),
					section("2017-08-02 07:00:00", "2017-08-02 12:00:00"),
					section("2017-08-02 18:00:00", "2017-08-03 00:00:00"),
					section("2017-08-03 00:00:00", "2017-08-03 07:00:00"),
					section("2017-08-03 07:00:00", "2017-08-03 12:00:00"),
					section("2017-08-03 12:00:00", "2017-08-03 18:00:00"),
					section("2017-08-03 18:00:00", "2017-08-03 18:10:00"),

				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-07-31 18:00:00", "2017-08-04 00:00:00"),
				},
				new[]
				{
					section( "2017-08-01 00:00:00", "2017-08-01 08:00:00"),
					section( "2017-08-01 08:00:00", "2017-08-01 20:00:00"),
					section( "2017-08-01 20:00:00", "2017-08-02 00:00:00"),
					section( "2017-08-02 00:00:00", "2017-08-02 07:00:00"),
					section( "2017-08-02 07:00:00", "2017-08-02 12:00:00"),
					section( "2017-08-02 18:00:00", "2017-08-03 00:00:00"),
					section( "2017-08-03 00:00:00", "2017-08-03 07:00:00"),
					section( "2017-08-03 07:00:00", "2017-08-03 12:00:00"),
					section( "2017-08-03 12:00:00", "2017-08-03 18:00:00"),
					section( "2017-08-03 18:00:00", "2017-08-04 00:00:00"),
				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-07-31 18:00:00", "2017-08-02 14:00:00"),
				},
				new[]
				{
					section( "2017-08-01 00:00:00", "2017-08-01 08:00:00"),
					section( "2017-08-01 08:00:00", "2017-08-01 20:00:00"),
					section( "2017-08-01 20:00:00", "2017-08-02 00:00:00"),
					section( "2017-08-02 00:00:00", "2017-08-02 07:00:00"),
					section( "2017-08-02 07:00:00", "2017-08-02 12:00:00"),
				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-08-02 00:00:00", "2017-08-02 13:00:00"),
				},
				new[]
				{
					section( "2017-08-02 00:00:00", "2017-08-02 07:00:00"),
					section( "2017-08-02 07:00:00", "2017-08-02 12:00:00"),
				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-08-02 14:00:00", "2017-08-04 00:00:00"),
				},
				new[]
				{
					section( "2017-08-02 18:00:00", "2017-08-03 00:00:00"),
					section( "2017-08-03 00:00:00", "2017-08-03 07:00:00"),
					section( "2017-08-03 07:00:00", "2017-08-03 12:00:00"),
					section( "2017-08-03 12:00:00", "2017-08-03 18:00:00"),
					section( "2017-08-03 18:00:00", "2017-08-04 00:00:00"),
				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-08-02 14:00:00", "2017-08-02 15:00:00"),
				},
				new DateTimeSection[0]
				{
				},
			};


			yield return new object[] 
			{
				_sourceList,
				new[]
				{
					section("2017-08-02 15:00:00", "2017-08-02 19:00:00"),
				},
				new[]
				{
					section( "2017-08-02 18:00:00", "2017-08-02 19:00:00"),
				},
			};

			yield return new object[] {
				_sourceList,
				new[]
				{
					section("2017-08-02 19:00:00", "2017-08-02 20:00:00"),
				},
				new[]
				{
					section( "2017-08-02 19:00:00", "2017-08-02 20:00:00"),
				},
			};
		}




		[Theory]
		[MemberData("IntersectSectionTestData")]
		public void IntersectSectionTest(IEnumerable<DateTimeSection> sourceA, IEnumerable<DateTimeSection> sourceB, IEnumerable<DateTimeSection> exceptList)
		{
			var actualList = DateTimeSectionExtensions.IntersectSection(sourceA, sourceB);
			Assert.Equal(exceptList, actualList);
		}



















	}
}
