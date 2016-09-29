using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Parse;
using System.Diagnostics;
using System.Collections;


namespace HookMeUP.iOS
{
	
	public partial class AdminViewController : ScreenViewController
	{
		public TableSourceAdmin Source
		{
			get;
			private set;
		}

		public Dictionary<string, OrdersAdmin> AdminGetOrders = new Dictionary<string, OrdersAdmin>();
		public List<string> Keys = new List<string>();

		IList orderItems = null;

		public string ChannelName
		{
			get;
			set;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			AddOrders();
		}


		public async void AddOrders()
		{
			AdminGetOrders.Clear();
			Keys.Clear();

			try
			{
				loadingOverlay = new LoadingOverlay(bounds);
				View.Add(loadingOverlay);

				ParseQuery<ParseObject> query = from ordersTb in ParseObject.GetQuery("Orders")
												where ordersTb.Get<bool>("IsOrderDone") == false
												select ordersTb;


				IEnumerable coll = await query.FindAsync();

				List<string> itemsOrdered;

				foreach (ParseObject parseObject in coll)
				{
					OrderReceivedByAdmin(parseObject);
					itemsOrdered = new List<string>();
					string objectId = parseObject.ObjectId;
					string personOrdered = parseObject.Get<string>("PersonOrdered");
					orderItems = parseObject.Get<IList>("OrderList");
					ChannelName = parseObject.Get<string>("UserChannel");
					foreach (string e in orderItems) itemsOrdered.Add(e);

					AdminGetOrders.Add(ChannelName, new OrdersAdmin(objectId, personOrdered, itemsOrdered));
					Keys.Add(ChannelName);
				}

				loadingOverlay.Hide();
				PopulateTable();
			}
			catch (ParseException e)
			{
				loadingOverlay.Hide();
				Debug.WriteLine(e.StackTrace);
			}

		}

		//public async void AddNewOrders() 
		//{
			
		//	LoadingOverlay loading = new LoadingOverlay(bounds);
		//	View.Add(loading);

		//	ParseQuery<ParseObject> query = from ordersTb in ParseObject.GetQuery("Orders")
		//									where ordersTb.Get<bool>("IsOrderDone") == false
		//									where ordersTb.Get<bool>("OrderReceivedByAdmin") == false
		//									select ordersTb;


		//	IEnumerable coll = await query.FindAsync();
		//	List<string> newOrders;

		//	foreach (ParseObject parseObject in coll)
		//	{
		//		newOrders = new List<string>();
		//		string objectId = parseObject.ObjectId;

		//		string personOrdered = parseObject.Get<string>("PersonOrdered");
		//		orderItems = parseObject.Get<IList>("OrderList");

		//		foreach (string e in orderItems)
		//		{
		//			Debug.WriteLine(e);
		//			newOrders.Add(e);
		//		}

		//		AdminGetOrders.Add(new OrdersAdmin(objectId, personOrdered, newOrders));

		//		OrderReceivedByAdmin(parseObject);
		//		loading.Hide();
		//		PopulateTable();
		//	}

		//}

		async void OrderReceivedByAdmin(ParseObject pObj)
		{
			if (!pObj.Get<bool>("OrderReceivedByAdmin"))
			{
				try
				{
					pObj["OrderReceivedByAdmin"] = true;
					await pObj.SaveAsync();
				}
				catch (ParseException e)
				{
					Debug.WriteLine(e.StackTrace + "Order received not updated");
				}
			}
			else Console.WriteLine("Order is received already");

		}

		void PopulateTable() 
		{
			if (orderItems != null) Source = new TableSourceAdmin(AdminGetOrders, Keys);
			else Debug.WriteLine("Order items is null");

			AminOrdersTable.Source = Source;
			AminOrdersTable.ReloadData();

		}

	}


	//==================================================================================================================
	//==================================================================================================================
	//==================================================================================================================


	public class TableSourceAdmin : UITableViewSource
	{
		string cellIdentifier = "TableCell";


		Dictionary<string, OrdersAdmin> Items
		{
			get;
		}
		string ChannelName
		{
			get;
			set;
		}

		string PersonOrderedName 
		{
			get;
			set;
		}

		List<string> Keys 
		{ 
			get;
		}
		public TableSourceAdmin(Dictionary<string, OrdersAdmin> items, List<string> keys)
		{
			Items = items;
			Keys = keys;

		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);


			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
			}
			string key = Keys[indexPath.Row];
			OrdersAdmin orders = Items[key];
			string personOrdered = orders.PersonOrdered;

			Debug.WriteLine(key);
			foreach (var e in Items)
				Debug.WriteLine(e.Key+"\t"+e.Value.Items);

			cell.TextLabel.Text = personOrdered;

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			string key = Keys[indexPath.Row];
			OrdersAdmin orders = Items[key];
			PersonOrderedName = orders.PersonOrdered;


			UIAlertView alert = new UIAlertView();
			alert.Title = "Order items";
			string s = "";

			foreach (string elements in orders.Items)
			{
				s += elements + "\n";
			}

			alert.Message = s.Trim();
			alert.AddButton("OK");
			alert.Show();
		}

		public override async void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
				case UITableViewCellEditingStyle.Delete:

					string key = Keys[indexPath.Row];
					OrdersAdmin orders = Items[key];
					string objectID = orders.ObjectId;
					Items.Remove(key);
					tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);

					try
					{
						ParseQuery<ParseObject> queryForUpdate = from ordersTB in ParseObject.GetQuery("Orders")
																 where ordersTB.Get<string>("objectId") == objectID
																 select ordersTB;

						ParseObject obj = await queryForUpdate.FirstAsync();
						obj["IsOrderDone"] = true;
						await obj.SaveAsync();

						//send push

						var push = new ParsePush();
						push.Data = new Dictionary<string, object>
						{
							{"title","HookMeUp"},
							{"alert","Your order is Ready!!!"},
							{"channel",ChannelName},
							{"badge","Increment"}

						};
						Debug.WriteLine(ChannelName);
						await push.SendAsync();

					}
					catch (ParseException e)
					{
						Debug.WriteLine(e.StackTrace);
					}
					break;

				case UITableViewCellEditingStyle.None:
					Debug.WriteLine("Nothing");
					break;
			}
		}

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
		{
			return "Done";
		}
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 70f;
		}
	}


}


