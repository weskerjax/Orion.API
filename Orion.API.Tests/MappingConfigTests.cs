using Orion.API.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System.Diagnostics;
using System.Data.Linq;

namespace Orion.API.Tests
{



	public class MappingConfigTests
	{


		[Fact]
		public void ObjectTest()
		{

			var fromObj = new UserDomain
			{
				UserId = 1,
				Name = "Jax",
				CreateDate = DateTime.Today,
				Column = new List<string>{ "OK" },
				Items = new List<UserItemDomain> 
				{ 
					new UserItemDomain { Name = "Yes" } 
				}
			};


			var toObj = fromObj.MappingModel<UserDomain, UserModel>(cfg => cfg
				.Mapping(x => x.Name, y => y.Name.Length.ToString())
				.Mapping(x => x.Items, y => y.Items.MappingModel<UserItemDomain, UserItemModel>())
				.IgnoreFrom(x => x.Named)
			);

			Assert.Equal("3", toObj.Name);
			Assert.Equal(fromObj.UserId, toObj.UserId);
			Assert.Equal(fromObj.CreateDate, toObj.CreateDate);
			Assert.Equal(fromObj.Items.Count, toObj.Items.Count); 
			
		}




		[Fact]
		public void ListTest()
		{
			var fromList = new List<UserDomain>{
				new UserDomain
				{
					UserId = 1,
					Name = "Jax",
					CreateDate = DateTime.Today,
				}
			};


			var toList = fromList.MappingModel<UserDomain, UserModel>(cfg => cfg
				.IgnoreBoth(x => x.Column)
				.IgnoreBoth(x => x.Items)
				.IgnoreFrom(x => x.Named)
			);


			Assert.Equal(fromList[0].UserId, toList[0].UserId);
			Assert.Equal(fromList[0].Name, toList[0].Name);
			Assert.Equal(fromList[0].CreateDate, toList[0].CreateDate); 
		}




		[Fact]
		public void StaticTest()
		{
			var fromList = Enumerable.Repeat(new UserModel
			{
				UserId = 1,
				Name = "Jax",
				CreateDate = DateTime.Today,
			}, 10000);


			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			string staticValue = "AnyText";
			var toList = fromList.MappingModel<UserModel, UserDomain>(cfg => cfg
				.Mapping(t => t.Named, f => staticValue)
				.IgnoreBoth(t => t.Items)
			);
			Assert.Equal(staticValue, toList[0].Named);

			stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			Console.Write(ts);
		}




		[Fact]
		public void ListStaticTest()
		{
			var fromList = new List<UserModel>{
				new UserModel
				{
					UserId = 11,
					Name = "Jax",
					CreateDate = DateTime.Today,
				},
				new UserModel
				{
					UserId = 12,
					Name = "Any",
					CreateDate = DateTime.Today,
				}
			};


			string staticValue = "AnyText";

			var toList = fromList.Select((x, i) => {
				int? userId = i;
				return x.MappingModel<UserModel, UserDomain>(cfg => cfg
					.Mapping(t => t.Named, f => staticValue)
					.Mapping(t => t.UserId, f => userId.Value)
					.IgnoreFrom(f => f.UserId)
					.IgnoreBoth(t => t.Items)
				);
			}).ToList();

			Assert.Equal(staticValue, toList[0].Named);
		}




		[Fact]
		public void LostFromTest()
		{
			var fromObj = new UserDomain();

			Assert.Throws(typeof(MappingException), () =>
			{
				var toObj = fromObj.MappingModel<UserDomain, UserModel>(cfg => cfg
					.IgnoreBoth(t => t.Items)
				);
			});
			
		}



		[Fact]
		public void LostToTest()
		{
			var fromObj = new UserModel();

			Assert.Throws(typeof(MappingException), () =>
			{
				var toObj = fromObj.MappingModel<UserModel, UserDomain>(cfg => cfg
					.IgnoreBoth(t => t.Items)
				);
			});
		}









	}





	public class UserDomain
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Named { get; set; }
		public DateTime CreateDate { get; set; }
		public List<string> Column { get; set; }
		public List<UserItemDomain> Items { get; set; }
	}

	public class UserItemDomain
	{
		public string Name { get; set; }
	}


	public class UserModel
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public DateTime CreateDate { get; set; }
		public List<string> Column { get; set; }
		public List<UserItemModel> Items { get; set; }
		public EntitySet<UserItemModel> EntitySet { get; set; }

	}

	public class UserItemModel
	{
		public string Name { get; set; }
	}


}
