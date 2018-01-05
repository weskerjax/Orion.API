using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;


namespace Orion.API.Extensions.Tests
{
	public class EnumerableExtensionsTests
	{
		[Fact]
		public void OrderBy_RunTest()
		{
			var list = new List<UserModel>
			{
				new UserModel{ UserId = 1, Name = "AB" },
				new UserModel{ UserId = 2, Name = "0A" },
			};
			list.OrderBy("UserId", true).ToList();
		}




		[Fact]
		public void ForEachTest()
		{
			var list = new List<UserModel>
			{
				new UserModel{ UserId = 1 },
				new UserModel{ UserId = 1 },
			};

			list = list
				.Select(x => x.Clone())
				.Each(x => x.UserId = 2)
				.ToList();

			Assert.Equal(2, list[0].UserId);
		}


	}

}
