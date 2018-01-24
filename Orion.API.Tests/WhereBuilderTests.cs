using Orion.API.Extensions;
using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Orion.API.Tests
{



	public class WhereBuilderTests
	{
		private OrionApiDataContext _dc;

		public WhereBuilderTests()
		{			
			string mdfPath = Path.GetFullPath(@"..\..\OrionApi.mdf");
			string connection = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True";
			_dc = new OrionApiDataContext(connection);
		}



		/*===========================================================================*/

		[Fact]		
		public void NullTest()
		{
			WhereParams<InvoiceIssueDomain> param = null;

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.InvoicePrefix, y => y.InvoicePrefix);

			var query = build.Build();
			var sql = query.ToString();

			Assert.NotEmpty(sql);
		}







		/*===========================================================================*/

		public static IEnumerable<object[]> StringValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (@p0)" };
				yield return new object[] { WhereOperator.NotIn, " IN (@p0)" };
				yield return new object[] { WhereOperator.Equals, " = @p0" };
				yield return new object[] { WhereOperator.NotEquals, " <> @p0" };
				yield return new object[] { WhereOperator.Contains, " LIKE @p0" };
				yield return new object[] { WhereOperator.StartsWith, " LIKE @p0" };
				yield return new object[] { WhereOperator.EndsWith, " LIKE @p0" };
				yield return new object[] { WhereOperator.LessThan, " < @p0" };
				yield return new object[] { WhereOperator.LessEquals, " <= @p0" };
				yield return new object[] { WhereOperator.GreaterThan, " > @p0" };
				yield return new object[] { WhereOperator.GreaterEquals, " >= @p0" };
				yield return new object[] { WhereOperator.Between, "Table(" };
			}
		}


		[Theory]
		[MemberData("StringValueTest_Data")]
		public void StringValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.InvoicePrefix, oper, "SS");

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.InvoicePrefix, y => y.InvoicePrefix);

			var query = build.Build();
			var sql = query.ToString();

			Assert.Contains(expected, sql);
		}




		/*===========================================================================*/

		public static IEnumerable<object[]> IntValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (@p0" };
				yield return new object[] { WhereOperator.NotIn, " IN (@p0" };
				yield return new object[] { WhereOperator.Equals, " = @p0" };
				yield return new object[] { WhereOperator.NotEquals, " <> @p0" };
				yield return new object[] { WhereOperator.Contains, "Table(" };
				yield return new object[] { WhereOperator.StartsWith, "Table(" };
				yield return new object[] { WhereOperator.EndsWith, "Table(" };
				yield return new object[] { WhereOperator.LessThan, " < @p0" };
				yield return new object[] { WhereOperator.LessEquals, " <= @p0" };
				yield return new object[] { WhereOperator.GreaterThan, " > @p0" };
				yield return new object[] { WhereOperator.GreaterEquals, " >= @p0" };
				yield return new object[] { WhereOperator.Between, " >= @p0" };
			}
		}


		[Theory]
		[MemberData("IntValueTest_Data")]
		public void IntValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQty, oper, 1, 3);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.ProductQty, y => y.InvoiceId);

			var query = build.Build();
			var sql = query.ToString();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/

		public static IEnumerable<object[]> DateTimeValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (@p0" };
				yield return new object[] { WhereOperator.NotIn, " IN (@p0" };
				yield return new object[] { WhereOperator.Equals, " = @p0" };
				yield return new object[] { WhereOperator.NotEquals, " <> @p0" };
				yield return new object[] { WhereOperator.Contains, "Table(" };
				yield return new object[] { WhereOperator.StartsWith, "Table(" };
				yield return new object[] { WhereOperator.EndsWith, "Table(" };
				yield return new object[] { WhereOperator.LessThan, " < @p0" };
				yield return new object[] { WhereOperator.LessEquals, " <= @p0" };
				yield return new object[] { WhereOperator.GreaterThan, " > @p0" };
				yield return new object[] { WhereOperator.GreaterEquals, " >= @p0" };
				yield return new object[] { WhereOperator.Between, " >= @p0" };
			}
		}


		[Theory]
		[MemberData("DateTimeValueTest_Data")]
		public void DateTimeValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ModifyDate, oper, DateTime.Today, DateTime.Now);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.ModifyDate, y => y.ModifyDate);

			var query = build.Build();
			var sql = query.ToString();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/

		public static IEnumerable<object[]> DecimalValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (@p0" };
				yield return new object[] { WhereOperator.NotIn, " IN (@p0" };
				yield return new object[] { WhereOperator.Equals, " = @p0" };
				yield return new object[] { WhereOperator.NotEquals, " <> @p0" };
				yield return new object[] { WhereOperator.Contains, "Table(" };
				yield return new object[] { WhereOperator.StartsWith, "Table(" };
				yield return new object[] { WhereOperator.EndsWith, "Table(" };
				yield return new object[] { WhereOperator.LessThan, " < @p0" };
				yield return new object[] { WhereOperator.LessEquals, " <= @p0" };
				yield return new object[] { WhereOperator.GreaterThan, " > @p0" };
				yield return new object[] { WhereOperator.GreaterEquals, " >= @p0" };
				yield return new object[] { WhereOperator.Between, " >= @p0" };
			}
		}


		[Theory]
		[MemberData("DecimalValueTest_Data")]
		public void DecimalValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.Sum, oper, 1.0m, 3.0m);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.Sum, y => y.Total);

			var query = build.Build();
			var sql = query.ToString();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/

		public static IEnumerable<object[]> SubQueryTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (@p0" };
				yield return new object[] { WhereOperator.NotIn, " IN (@p0" };
				yield return new object[] { WhereOperator.Equals, " = @p0" };
				yield return new object[] { WhereOperator.NotEquals, " <> @p0" };
				yield return new object[] { WhereOperator.Contains, "Table(" };
				yield return new object[] { WhereOperator.StartsWith, "Table(" };
				yield return new object[] { WhereOperator.EndsWith, "Table(" };
				yield return new object[] { WhereOperator.LessThan, " < @p0" };
				yield return new object[] { WhereOperator.LessEquals, " <= @p0" };
				yield return new object[] { WhereOperator.GreaterThan, " > @p0" };
				yield return new object[] { WhereOperator.GreaterEquals, " >= @p0" };
				yield return new object[] { WhereOperator.Between, " >= @p0" };
			}
		}


		[Theory]
		[MemberData("SubQueryTest_Data")]
		public void SubQueryTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQty, oper, 1, 3);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind<int?>(x => x.ProductQty, y => y.InvoiceIssueItems.Select(z => (int?)z.Qty));

			var query = build.Build();
			var sql = query.ToString();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/

		[Fact]
		public void ListTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.SetValues(x => x.RoleIds, WhereOperator.Equals, 1);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.RoleIds, y => y.InvoiceIssueItems.Select(z => z.Qty))
				.Build();

			var sql = query.ToString();

			query.ToList();


			Assert.True(true);
		}





		/*===========================================================================*/

		[Fact]
		public void ConvertTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.SetValues(x => x.UseStatus, WhereOperator.Equals, UseStatus.Enable)
				.SetValues(x => x.ProductQty, WhereOperator.Equals, 1);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.UseStatus.ToString(), y => y.InvoicePrefix)
				.WhereBind(x => x.ProductQty * 100, y => y.InvoiceId)
				.Build();

			var sql = query.ToString();

			query.ToList();

			Assert.True(true);
		}





		/*===========================================================================*/

		[Fact]
		public void StringTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.Assign(x => x.InvoicePrefix == null);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.UseStatus.ToString(), y => y.InvoicePrefix)
				.WhereBind(x => x.InvoicePrefix, y => y.InvoicePrefix)
				.Build();

			var sql = query.ToString();

			query.ToList();


			Assert.True(true);
		}


	}




	public enum UseStatus
	{
		Enable,
		Disable,

	}

	public class InvoiceIssueDomain
	{
		public int? ProductQty { get; set; }
		public decimal Sum { get; set; }
		public string InvoicePrefix { get; set; }
		public UseStatus UseStatus { get; set; }
		public int ModifyBy { get; set; }
		public DateTime ModifyDate { get; set; }
		public List<int> RoleIds { get; set; }
	}

}
