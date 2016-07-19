using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class OrderViewController : ScreenViewController
	{
		
		public TableSource Source { get; set; }


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			DismissKeyboardOnBackgroundTap();
			//inserting cells into the table
			List<string> tableItems = new List<string>() {"Hot Chocolate","Espresso","Red Espresso","Cafe Americano","Cafe Mocha","Cappuccino","Flavoured Cappuccino","Red Cappuccino","Latte","Flavoured Latte","Red Latte"};

			Source = new TableSource(tableItems);

			ordersTable.Source = Source;

			hookMeUPButton.TouchUpInside += (obj, evt) => {
				// getting orders

				if (Source.ordersList != null)
				{
					string elements = "";
				
					foreach (string orderElements in Source.ordersList) {
						elements += orderElements+"\n";
					
					}
					AlertPopUp("Orders selected",elements.Trim(),"OK");


				}
				else
				{

					AlertPopUp("Error", "No order(s) selected", "OK");

				}
			
			};
		}

	}


	//==============================================================================================================================================================================================================================



	public class TableSource : UITableViewSource
	{
		List<string> tableItems;
		string cellIdentifier = "TableCell";
		public string order = "";

		public TableSource(List<string> items) {
			tableItems = items;
		}


		public List<string> ordersList = new List<string>();

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
			return tableItems.Count;
		}



		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			ordersList.Add(tableItems[indexPath.Row]);
		}

		public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
		{
			ordersList.Remove(tableItems[indexPath.Row]);
		}



	}
}


