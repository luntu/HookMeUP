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

			AdminGetOrders.Clear();		

			try
			{
				loadingOverlay = new LoadingOverlay(bounds);
				View.Add(loadingOverlay);

				ParseQuery<ParseObject> query = ParseObject.GetQuery("Orders");
				query.Include("PersonOrdered").Include("OrderList");
				IEnumerable coll = await query.FindAsync();
			
				string orderConcat = "";

				foreach (ParseObject parseObject in coll) 
				{
					string personOrdered = parseObject.Get<string>("PersonOrdered");
					IList orderItems = parseObject.Get<IList>("OrderList");

					foreach (string e in orderItems) {
						orderConcat += e + "+";
					}

					AdminGetOrders.Add(personOrdered + "#" + orderConcat.Trim());
					orderConcat = String.Empty;
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

		List<string> orders= new List<string>();
	

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
			orders.Add(split[1]);

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


			string[] split = orders[indexPath.Row].Split('+');
			string s = "";
			foreach (string elements in split) 
			{
				s += elements+"\n";
			}
			alert.Message = s.Trim();
			alert.AddButton("OK");
			alert.Show();
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			switch (editingStyle)
			{ 
				case UITableViewCellEditingStyle.Delete:
					items.RemoveAt(indexPath.Row);
					tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
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


