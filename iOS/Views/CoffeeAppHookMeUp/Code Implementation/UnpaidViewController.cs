using System;
using System.Diagnostics;
using System.Collections.Generic;
using Foundation;
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class UnpaidViewController : ScreenViewController
	{

		TableSourceUnpaid Source 
		{
			get;
			set;
		}

		Dictionary<string, double> OrdersMap 
		{
			get;
			set;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			backButton.TouchUpInside += (sender, e) =>
			{
				NavigationController.PopViewController(true);
			};
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			ShowUnpaid();
		}

		async void ShowUnpaid()
		{
			LoadingOverlay lo = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(lo);
			try
			{
				var query = from unpaid in ParseObject.GetQuery("Unpaid")
							where unpaid.Get<bool>("Paid") == false
							select unpaid;

				var results = await query.FindAsync();

				UnpaidContainer container = new UnpaidContainer();

				foreach (var elements in results)
				{
					container.Order = new OrdersAdmin(elements.ObjectId,
												  elements.Get<string>("Name"),
												  elements.Get<string>("UserChannel"),
												  elements.Get<double>("AmountOwing"));
					container.InitialiseMaps();
				}

				OrdersMap = container.GetOrdersMap;
				Source = new TableSourceUnpaid(OrdersMap);
				unpaidTable.Source = Source;
				unpaidTable.ReloadData();
			}
			catch (ParseException ex)
			{
				Debug.WriteLine(ex.Message);
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex.GetType()+"\n"+ex.Message);
			}
			lo.Hide();

		}
	}

	//==================================================================================================================
	//==================================================================================================================
	//==================================================================================================================

	class TableSourceUnpaid : UITableViewSource
	{
		string cellIdentifier = "TableCell";

		Dictionary<string, double> OrdersMap 
		{
			get;
		}

		List<string> Keys
		{
			get;
			set;
		}

		internal TableSourceUnpaid(Dictionary<string, double> ordersMap)
		{
			OrdersMap = ordersMap;
			foreach (string elementKeys in ordersMap.Keys)
			{
				Keys.Add(elementKeys);
				Debug.WriteLine(elementKeys);
			}

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);

			cell.TextLabel.Text = Keys[indexPath.Row];
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Keys.Count;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 70f;
		}
	}
}

