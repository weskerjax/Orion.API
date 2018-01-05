using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;


namespace Orion.API.Extensions.Tests
{
	public class QueryableExtensionsTests
	{
		[Fact]
		public void OrderBy_RunTest()
		{
			var list = new List<UserModel>
			{
				new UserModel{ UserId = 1 },
				new UserModel{ UserId = 2 },
			};

			list.AsQueryable().OrderBy("UserId", true).ToList();
		}

		[Fact]
		public void MaxOrDefault_RunTest()
		{
			new List<UserModel> { }.AsQueryable().MaxOrDefault(x => x.UserId);
			new List<UserModel> { new UserModel { UserId = 2 } }.AsQueryable().MaxOrDefault(x => x.UserId);
		}

		[Fact]
		public void MinOrDefault_RunTest()
		{
			new List<UserModel> { }.AsQueryable().MinOrDefault(x => x.UserId);
			new List<UserModel> { new UserModel{ UserId = 2 } }.AsQueryable().MinOrDefault(x => x.UserId);
		}

	}

}
