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
		public List<OrdersAdmin> AdminGetOrders = new List<OrdersAdmin>();

		IList orderItems = null;

		public string ChannelName
		{
			get;
			private set;
		}

		public AdminViewController(string channelName)
		{
			ChannelName = channelName;
		}

		public AdminViewController()
		{
		}
	
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			AddOrders();
		}


		public async void AddOrders() 
		{
			AdminGetOrders.Clear();

			try
			{
				loadingOverlay = new LoadingOverlay(bounds);
				View.Add(loadingOverlay);

				ParseQuery<ParseObject> query = from ordersTb in ParseObject.GetQuery("Orders")
												where ordersTb.Get<bool>("IsOrderDone") == false
												select ordersTb;


				IEnumerable coll = await query.FindAsync();

				List<string> itemsOrdered = new List<string>();
				foreach (ParseObject parseObject in coll)
				{
					string objectId = parseObject.ObjectId;

					string personOrdered = parseObject.Get<string>("PersonOrdered");
					orderItems = parseObject.Get<IList>("OrderList");

					foreach (string e in orderItems)
					{
						itemsOrdered.Add(e);
						//orderConcat += e + "+";
					}
					AdminGetOrders.Add(new OrdersAdmin(objectId,personOrdered, itemsOrdered));
					//AdminGetOrders.Add(objectId + "#" + personOrdered + "#" + orderConcat.Trim());
					itemsOrdered.Clear();
				}


				loadingOverlay.Hide();
			}
			catch (ParseException e)
			{
				loadingOverlay.Hide();
				Debug.WriteLine(e.StackTrace);
			}
			if (orderItems != null)
			{
				Source = new TableSourceAdmin(AdminGetOrders, ChannelName);
			}
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


		List<OrdersAdmin> Items
		{
			get;
			set;
		}
		string ChannelName 
		{
			get;
			set;
		}
		//List<string> Orders= new List<string>();

		string PersonOrderedName = "";


		public TableSourceAdmin(List<OrdersAdmin> items,string channelName) {
			Items = items;
			ChannelName = channelName;
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);


			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
			}
			//string[] split = Items[indexPath.Row].Split('#');
			//string objectIdd = split[0];
			//string personOrdered = split[1];
			//Orders.Add(objectIdd+"-"+personOrdered + "-" + split[2]);

			//cell.TextLabel.Text = personOrdered;

			OrdersAdmin orders = Items[indexPath.Row];
			string personOrdered = orders.PersonOrdered;

			cell.TextLabel.Text = personOrdered;

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			//string[] split = Orders[indexPath.Row].Split('-');
			
			//PersonOrderedName = split[1];
			//string[] split1 = split[2].Split('+');

			OrdersAdmin orders = Items[indexPath.Row];
			PersonOrderedName = orders.PersonOrdered;
			Debug.WriteLine(PersonOrderedName);

			UIAlertView alert = new UIAlertView();
			alert.Title = "Order items";
			string s = "";

			foreach (string elements in orders.Items) 
			{
				Debug.WriteLine(elements);
				s += elements+"\n";
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
					Debug.WriteLine(PersonOrderedName);

					//string[] split = Items[indexPath.Row].Split('#');
					//string objectID = split[0];
					OrdersAdmin orders = Items[indexPath.Row];
					string objectID = orders.ObjectId;
					Items.Remove(orders);
					tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);

					try
					{
					ParseQuery<ParseObject> queryForUpdate = from ordersTB in ParseObject.GetQuery("Orders")
						                                     where ordersTB.Get<string>("objectId") == objectID
															 select ordersTB;

						ParseObject obj =  await queryForUpdate.FirstAsync();
						obj["IsOrderDone"] = true;
						await obj.SaveAsync();
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


