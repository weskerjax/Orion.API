using Orion.API.Extensions;
using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Orion.API.Models.Tests
{



	public class WhereParamsTests
	{
	 

		[Fact]
		public void CreateByObject_Test()
		{
			var find = new InvoiceIssueDomain(){
				ProductQty = 1,
				GroupCode = Guid.NewGuid(),
			};

			var param = WhereParams.CreateByObject(find);

			Assert.True(param.GetValues(x => x.ProductQty).Length > 0);
			Assert.True(param.GetValues(x => x.Sum).Length == 0);
		}




		[Fact]
		public void SetValues_IntTest1()
		{
			IWhereParams param = WhereParams.CreateByObject(new InvoiceIssueDomain());
			param.SetValues("ProductQty", WhereOperator.In, new[] { 1, 2, 3, 4 });

			var values = param.GetValues("ProductQty");
			Assert.Equal(4, values.Length);
		}

		[Fact]
		public void SetValues_IntTest2()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQty, WhereOperator.In, new int?[] { 1, 2, 3, 4 });

			var values = param.GetValues(x => x.ProductQty);
			Assert.Equal(4, values.Length);
		}

		[Fact]
		public void SetValues_IntTest3()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQty, WhereOperator.In, 22);

			var values = param.GetValues(x => x.ProductQty);
			Assert.Equal(1, values.Length);
		}

		[Fact]
		public void SetValues_IntTest4()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.InvoicePrefix, WhereOperator.Equals, null);
			param.SetValues(x => x.ProductQty.Value, WhereOperator.In, 22);

			var values = param.GetValues(x => x.ProductQty);
			Assert.Equal(1, values.Length);
		}


		[Fact]
		public void SetValues_IntTest5()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQtys, WhereOperator.Equals, 22);

			var values = param.GetValues(x => x.ProductQtys);
			Assert.Equal(1, values.Length);
		}



		[Fact]
		public void SetValues_StringTest1()
		{
			IWhereParams param = WhereParams.CreateByObject(new InvoiceIssueDomain());
			param.SetValues("InvoicePrefix", WhereOperator.In, new[] { "1222" });
			var values = param.GetValues("InvoicePrefix");
			Assert.Equal(1, values.Length);
		}

		[Fact]
		public void SetValues_StringTest2()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.InvoicePrefix, WhereOperator.In, new[] { "1222" });
			var values = param.GetValues(x => x.InvoicePrefix);
			Assert.Equal(1, values.Length);
		}

		[Fact]
		public void SetValues_StringTest3()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.InvoicePrefix, WhereOperator.In, "1222");
			var values = param.GetValues(x => x.InvoicePrefix);
			Assert.Equal(1, values.Length);
		}

		[Fact]
		public void SetValues_StringTest4()
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.InvoicePrefix, WhereOperator.Equals, null);
			var values = param.GetValues(x => x.InvoicePrefix);
			Assert.Equal(0, values.Length);
		}


		private string _prefix = "OK";

		[Fact]
		public void Assing_Test1()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.InvoicePrefix == _prefix);
			Assert.Equal(1, param.GetValues(x => x.InvoicePrefix).Length);
			Assert.Equal(WhereOperator.Equals, param.GetOperator(x => x.InvoicePrefix));
		}

		[Fact]
		public void Assing_Test2()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.Sum > int.Parse("1"));
			Assert.Equal(1, param.GetValues(x => x.Sum).Length);
			Assert.Equal(WhereOperator.GreaterThan, param.GetOperator(x => x.Sum));			
		}

		[Fact]
		public void Assing_Test3()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.UseStatus == "Enable".ToEnum<UseStatus>());
			Assert.Equal(1, param.GetValues(x => x.UseStatus).Length);
			Assert.Equal(WhereOperator.Equals, param.GetOperator(x => x.UseStatus));
			Assert.Equal(UseStatus.Enable, param.GetValues(x => x.UseStatus)[0]);
		}

		[Fact]
		public void Assing_Test4()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.UseStatus.IsIn(UseStatus.Enable));
			Assert.Equal(1, param.GetValues(x => x.UseStatus).Length);
			Assert.Equal(WhereOperator.In, param.GetOperator(x => x.UseStatus));
			Assert.Equal(UseStatus.Enable, param.GetValues(x => x.UseStatus)[0]);
		}

		[Fact]
		public void Assing_Test5()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => new List<int?> { 1, 2 }.Contains(x.ModifyBy));
			Assert.Equal(2, param.GetValues(x => x.ModifyBy).Length);
			Assert.Equal(WhereOperator.In, param.GetOperator(x => x.ModifyBy));
		}

		[Fact]
		public void Assing_Test6()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => !new int?[] { 1, 2, 3 }.Contains(x.ProductQty));
			Assert.Equal(3, param.GetValues(x => x.ProductQty).Length);
			Assert.Equal(WhereOperator.NotIn, param.GetOperator(x => x.ProductQty));
		}


		[Fact]
		public void Assing_Test7()
		{
			var list = new List<InvoiceIssueDomain> { new InvoiceIssueDomain { Sum = 1 } };

			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.Sum > list[0].Sum);
			Assert.Equal(1, param.GetValues(x => x.Sum).Length);
			Assert.Equal(WhereOperator.GreaterThan, param.GetOperator(x => x.Sum));
		}





		[Fact]
		public void Assing_Test8()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.InvoicePrefix == null);
			Assert.Equal(1, param.GetValues(x => x.InvoicePrefix).Length);
			Assert.Equal(WhereOperator.Equals, param.GetOperator(x => x.InvoicePrefix));
		}

		[Fact]
		public void Assing_Test9()
		{
			var param = new WhereParams<InvoiceIssueDomain>().Assign(x => x.InvoicePrefix != null);
			Assert.Equal(1, param.GetValues(x => x.InvoicePrefix).Length);
			Assert.Equal(WhereOperator.NotEquals, param.GetOperator(x => x.InvoicePrefix));
		}




	}


	public class InvoiceIssueDomain
	{
		public List<int> ProductQtys { get; set; }
		public int? ProductQty { get; set; }
		public Guid? GroupCode { get; set; }
		public decimal? Sum { get; set; }
		public string InvoicePrefix { get; set; }
		public UseStatus? UseStatus { get; set; }
		public int? ModifyBy { get; set; }
		public DateTime? ModifyDate { get; set; }
	}

}
