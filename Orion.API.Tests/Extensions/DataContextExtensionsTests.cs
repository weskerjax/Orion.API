using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Transactions;
using Xunit;
using System.Data.SQLite;
using Orion.API.Tests;
using System.IO;

namespace Orion.API.Extensions.Tests
{

	public class DataContextExtensionsTests
	{
		private OrionApiDataContext _dc;

		public DataContextExtensionsTests()
		{
			string mdfPath = Path.GetFullPath(@"..\..\OrionApi.mdf");
			string connection = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True";
			_dc = new OrionApiDataContext(connection);

		}



		[Fact]
		public void CreateTable_RunTest()
		{
			List<InventoryTemp> list = null;

			using (var tx = new TransactionScope())
			{
				if (!_dc.TableExists<InventoryTemp>())
				{
					_dc.InventoryTemp.Create();
				}


				_dc.InventoryTemp.InsertOnSubmit(new InventoryTemp
				{
					MaterialCode = "OK",
					BranchFactory = "OK",
					ZoneCode = "OK",
					BatchCode = "OK",
					Quantity = 1,
					ModifyDate = DateTime.Now,
				});

				_dc.SubmitChanges();

				list = _dc.InventoryTemp.ToList();

				tx.Complete();
			}

			Assert.NotNull(list);
		}



		[Fact]
		public void CreateTable_SqliteRunTest()
		{
			var cntString = new SQLiteConnection("Data Source=database.sqlite");
			var dc = new OrionApiSqliteDataContext(cntString);


			bool exists = dc.TableExists<PrintQueue>();
			if (!exists) { dc.PrintQueue.Create(); }


			dc.PrintQueue.InsertOnSubmit(new PrintQueue
			{
				CreateTime = DateTime.Now,
				CommandNo = "OK",
				CommandStatus = "OK",
				PrintId = "OK",
				TempName = "OK",
				PrintInfo = "OK",
				Prefix = "OK",
				StartNo = 1,
				EndNo = 1,
				NowNo = 1,
			});

			dc.SubmitChanges();
		}



		
		[Fact]
		public void Replace_RunTest()
		{
			/* 準備資料 */
			var data = new InvoiceIssue
			{
				InvoicePrefix = "OK",
				InvoiceNum = (int)(DateTime.Now - new DateTime(2016,1,1)).TotalSeconds,
				InvoiceDate = DateTime.Now,
				DeliveryCustCode = "OK",
				DeliveryCustName = "OK",
				Total = 1,
				CreateBy = 1,
				CreateDate = DateTime.Now,
				ModifyBy = 1,
				ModifyDate = DateTime.Now,
			};

			data.InvoiceIssueItems.Add(new InvoiceIssueItems 
			{ 
				DeliveryNum = "OK",
				PurchaseNum = "OK",
				Qty = 1,
				Price = 1,
				TotalPrice =1,
			});
			data.InvoiceIssueItems.Add(new InvoiceIssueItems
			{
				DeliveryNum = "OK1",
				PurchaseNum = "OK1",
				Qty = 1,
				Price = 1,
				TotalPrice = 1,
			});

			_dc.InvoiceIssue.InsertOnSubmit(data);
			_dc.SubmitChanges();



			/*################################################*/

			/* 測試資料替換 */
			var dbData = _dc.InvoiceIssue.Single(x => x.InvoiceId == data.InvoiceId);

			var items = new List<InvoiceIssueItems>
			{
				new InvoiceIssueItems 
				{ 
					DeliveryNum = "Yes",
					PurchaseNum = "Yes",
					Qty = 2,
					Price = 2,
					TotalPrice = 2,
				},
				dbData.InvoiceIssueItems.First(),
				new InvoiceIssueItems 
				{ 
					DeliveryNum = "Yes",
					PurchaseNum = "Yes",
					Qty = 2,
					Price = 2,
					TotalPrice = 2,
				}
			};

			_dc.InvoiceIssueItems.Replace(dbData.InvoiceIssueItems, items);
			_dc.SubmitChanges();

			var g = _dc.InvoiceIssueItems.ToList();

		}


	}
}
