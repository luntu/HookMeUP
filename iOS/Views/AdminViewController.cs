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

		public TableSourceAdmin Source { get; set; }
		public List<string> AdminGetOrders { get; set; } = new List<string>();

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();


		

			try
			{
				loadingOverlay = new LoadingOverlay(bounds);
				View.Add(loadingOverlay);

				ParseQuery<ParseObject> query = ParseObject.GetQuery("Orders");
				query.Include("PersonOrdered").Include("OrderList");
				var coll = await query.FindAsync();
				string orderConcat = "";
				foreach (var parseObject in coll) 
				{
					string personOrdered = parseObject.Get<string>("PersonOrdered");
					IList orderItems = parseObject.Get<IList>("OrderList");

					foreach (string e in orderItems) {
						orderConcat += e + " ";
					}

					AdminGetOrders.Add(personOrdered + " # " + orderConcat.Trim());

            	}



				loadingOverlay.Hide();
			}
			catch (ParseException e)
			{
				loadingOverlay.Hide();
				Debug.WriteLine(e.StackTrace);
			}

			Source = new TableSourceAdmin(AdminGetOrders);
			AminOrdersTable.Source = Source;
			AminOrdersTable.ReloadData();

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}


	}

	public class TableSourceAdmin : UITableViewSource
	{
		string cellIdentifier = "TableCell";

		List<string> items;

		string orders;

		public TableSourceAdmin(List<string> items) {
			this.items = items;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);


			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);

			}
			string[] split = items[indexPath.Row].Split('#');

			string personOrdered = split[0];
			orders = split[1];
			cell.TextLabel.Text = personOrdered;

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return items.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Order items";

			string s = "";
		
			foreach (string e in orders.Split(' ')) {
				s = e+"\n";

			}
			alert.Message = s.Trim();
			alert.AddButton("OK");
			alert.Show();
		}
	}


}


