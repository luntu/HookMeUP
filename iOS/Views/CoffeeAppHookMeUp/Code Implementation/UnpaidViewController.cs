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
					string objectID = elements.ObjectId;
					string name = elements.Get<string>("Name");
					string userChannel = elements.Get<string>("UserChannel");
					double amountOwing = elements.Get<double>("AmountOwing");

					container.Order = new OrdersAdmin(objectID, name, userChannel, amountOwing);
					container.InitialiseMaps();
					container.InitialiseList(elements);
				}

				Source = new TableSourceUnpaid(container.GetOrdersMap, container.GetParseObjectList ,container.GetUnaddedOrders);
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

		List<ParseObject> ObjectParse
		{
			get;
		}

		List<string> Unadded 
		{
			get;
		}

		List<string> Keys = new List<string>();

		internal TableSourceUnpaid(Dictionary<string, double> ordersMap,List<ParseObject> objectParse, List<string> unadded)
		{
			ObjectParse = objectParse;
			OrdersMap = ordersMap;
			Unadded = unadded;

			foreach (string elementKeys in ordersMap.Keys)
				Keys.Add(elementKeys);
			
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
			
			string channelKey = Keys[indexPath.Row].Split('-')[0];

			cell.TextLabel.Text = Keys[indexPath.Row].Split('-')[1];

			cell.DetailTextLabel.Text = (OrdersMap[Keys[indexPath.Row]]+CalculatePrice(channelKey)).ToString("R 0.00");
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return OrdersMap.Count;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 70f;
		}

		public override async void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
				case UITableViewCellEditingStyle.Delete:

					int index = indexPath.Row;
					OrdersMap.Remove(Keys[index]);
					tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					UpdateDuplicates(ObjectParse[index]);
					break;
			}
		}

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
		{
			return "Paid";
		}

		private double CalculatePrice(string key)
		{
			double price = 0;

			foreach (string elements in Unadded)
			{
				string channelKey = elements.Split('-')[0];
				string priceValue = elements.Split('-')[1];

				if (channelKey.Equals(key))
					price += double.Parse(priceValue);

			}
			return price;
		}

		private async void UpdateDuplicates(ParseObject parseObject)
		{

			string channelName = parseObject.Get<string>("UserChannel");

			foreach (var elementObject in ObjectParse)
			{
				Debug.WriteLine(elementObject.Get<string>("UserChannel"));
				string channelName1 = elementObject.Get<string>("UserChannel");

				if (channelName1.Equals(channelName))
				{
					try
					{
						elementObject["Paid"] = true;
						await elementObject.SaveAsync();
					}
					catch (ParseException ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}
			}

		}
	}
}

