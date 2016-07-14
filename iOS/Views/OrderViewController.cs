using System;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class OrderViewController : UIViewController
	{
		

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.



			string[] tableItems = new string[] {"Hot Chocolate","Espresso","Red Espresso","Cafe Americano","Cafe Mocha","Cappuccino","Flavoured Cappuccino","Red Cappuccino","Latte","Flavoured Latte","Red Latte"};
			ordersTable.Source = new TableSource(tableItems);

			NavigationController.NavigationBarHidden = true;

		}

	}



	public class TableSource : UITableViewSource
	{
		string[] tableItems;
		string cellIdentifier = "TableCell";
		public TableSource(string[] items)
		{
			tableItems = items;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
			string item = tableItems[indexPath.Row];

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

			}
			cell.TextLabel.Text = item;
			return cell;
		}


		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Length;
		}
	}
}


