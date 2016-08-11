using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Parse;
using System.Diagnostics;

namespace HookMeUP.iOS
{
	public partial class QueueViewController : ScreenViewController
	{

		List<string> activeOrdersList;
		public TableSourceActiveOrders Source { get; set; }

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			activeOrdersList = new List<string>();

			loadingOverlay = new LoadingOverlay(bounds);
			View.Add(loadingOverlay);

			ParseQuery<ParseObject> query = from ordersTB in ParseObject.GetQuery("Orders")
											where ordersTB.Get<bool>("IsOrderDone") == false
											select ordersTB;

			IEnumerable<ParseObject> column = await query.FindAsync();

			foreach (ParseObject nameElements in column)
			{
				string nameOfCurrentUser = orderViewController.GetName;
				string namesColumn = nameElements.Get<string>("PersonOrdered");
				string replacedName = namesColumn.Replace(nameOfCurrentUser, "You");
				activeOrdersList.Add(replacedName);
			}
			loadingOverlay.Hide();

			Source = new TableSourceActiveOrders(activeOrdersList);

			ActiveOrdersTable.Source = Source;


			backOrdersButton.TouchUpInside += (o, e) =>
			{
				NavigationController.PopViewController(true);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

	}



	public class TableSourceActiveOrders : UITableViewSource
	{
		string cellIdentifier = "TableCell";
		List<string> itemList;

		public TableSourceActiveOrders(List<string> items)
		{
			itemList = items;
			foreach (string g in itemList) 
			{
				Debug.WriteLine(g);
			}
		}
		
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
			string item = itemList[indexPath.Row];

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);

			}

			if (item.Equals("You"))
			{
				cell.BackgroundColor = UIColor.Green;
			}
			else {
				cell.BackgroundColor = UIColor.LightGray;
			}

			cell.TextLabel.Text = item;
			cell.DetailTextLabel.Text = "Order number: " + (indexPath.Row+1);

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return itemList.Count;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return "Active orders\n";
		}
	}
}


