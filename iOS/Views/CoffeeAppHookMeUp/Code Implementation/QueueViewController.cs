using System;
using System.Collections.Generic;
using Foundation;
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class QueueViewController : ScreenViewController
	{

		public List<string> ActiveOrdersList = new List<string>();

		public TableSourceActiveOrders Source 
		{
			get;
			private set;
		}

		public override  void ViewDidLoad()
		{
			// Perform any additional setup after loading the view, typically from a nib.

			backOrdersButton.TouchUpInside += (o, e) =>
			{
				NavigationController.PopViewController(true);
			};
		}

		public override void ViewDidAppear(bool animated)
		{
			AddQueueOrders();		
		}

		public async void AddQueueOrders() 
		{
			loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(loadingOverlay);

			ActiveOrdersList.Clear();

			ParseQuery<ParseObject> query = from ordersTB in ParseObject.GetQuery("Orders")
											where ordersTB.Get<bool>("IsOrderDone") == false
											select ordersTB;

			IEnumerable<ParseObject> column = await query.FindAsync();
			loadingOverlay.Hide();

			foreach (ParseObject nameElements in column)
			{
				string nameOfCurrentUser = orderViewController.GetName;
				string namesColumn = nameElements.Get<string>("PersonOrdered");
				string replacedName = namesColumn.Replace(nameOfCurrentUser, "You");
				string time = nameElements.Get<string>("Time");
				ActiveOrdersList.Add(replacedName + "#" + time);
			}
			PopulateTable();
		}

		void PopulateTable()
		{
			Source = new TableSourceActiveOrders(ActiveOrdersList);
			ActiveOrdersTable.Source = Source;
			ActiveOrdersTable.ReloadData();
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

		}
	
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
			string item = itemList[indexPath.Row];
			string[] split = item.Split('#');
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);

			}

			if (split[0].Equals("You"))
			{
				cell.BackgroundColor = UIColor.Green;
			}
			else {
				cell.BackgroundColor = UIColor.LightGray;
			}
			cell.Layer.CornerRadius = 3f;
			cell.TextLabel.Text ="#" + (indexPath.Row + 1) + " " + split[0];
			cell.DetailTextLabel.Text = "Time: " + split[1] + " minutes";

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


