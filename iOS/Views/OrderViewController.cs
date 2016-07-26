using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class OrderViewController : ScreenViewController
	{

		public TableSource Source { get; set; }
		double dynamicPrice = 0.00;


		List<string> tableItems = new List<string>() { "Espresso#15.00", "Red Espresso#15.50", "Cappuccino#19.00",
		"Red Cappuccino#19.50", "Vanilla Cappuccino#28.00", "Hazelnut Cappuccino#28.00", "Latte#22.50", "Red Latte#20.00",
		"Vanilla Latte#30.00", "Hazelnut Latte#30.00", "Cafe Americano#18.50", "Cafe Mocha#24.50", "Hot Chocolate#20.00" };
		OrderWaitTime orderWaitTime = new OrderWaitTime();

		public int GetVouchers { get; set; }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			//inserting cells into the table

			VouchersLabel.Text = GetVouchers + " vouchers";
			Source = new TableSource(tableItems);
			Source.Voucher = VouchersLabel.Text;


			Source.onCellSelectedForPrice += (o, e) =>
			{
				DisplaySelectedOrderPrice(e);
			};


			Source.onCellDeselectedForPrice += (o, e) =>
			 {
				 DeductDeselectedOrderPrice(e);
			 };

		
			Source.onCellSelectedForVouchers += (o, e) =>
			 {
				 e--;
				 VouchersLabel.Text = e + " Vouchers";
				 Source.Voucher = VouchersLabel.Text;
			 };

		
			Source.onCellDeselectedForVouchers += (o, e) =>
			{
				e++;
				VouchersLabel.Text = e + " Vouchers";
				Source.Voucher = VouchersLabel.Text;
			};

			ordersTable.Source = Source;



			hookMeUPButton.TouchUpInside += (obj, evt) =>
			{
				// getting orders

				try
				{
					if (Source.ordersList != null && !Source.ordersList[0].Equals(""))
					{

						string elements = "";

						foreach (string orderElements in Source.ordersList)
						{
							string[] split = orderElements.Split('#');
							elements += split[0] + "\n";
						}

						UIAlertView alert = new UIAlertView();
						alert.Title = "Orders selected";
						alert.Message = elements.Trim();
						alert.AddButton("Order");
						alert.AddButton("Cancel");
						alert.Clicked += (o, e) =>
						{
							if (e.ButtonIndex == 0)
							{
								//submit datadase. Notify Vuyo
								orderWaitTime.GetOrdersTotal = Source.ordersList.Count;

								AlertPopUp("Order on the way", "Your order will take about " + orderWaitTime.CalculateWaitTime() + " minutes", "OK");
							}

						};
						alert.Show();


					}
					else
					{

						AlertPopUp("Error", "No order(s) selected", "OK");

					}
				}
				catch (ArgumentOutOfRangeException)
				{
					AlertPopUp("Error", "No order(s) selected", "OK");

				}

			};

		}

		public void DisplaySelectedOrderPrice(double price)
		{
			dynamicPrice += price;
			costText.Text = dynamicPrice.ToString("R 0.00");

		}
		public void DeductDeselectedOrderPrice(double price)
		{

			dynamicPrice -= price;

			costText.Text = dynamicPrice.ToString("R 0.00");
		}

	}

	//==============================================================================================================
	//==============================================================================================================
	//==============================================================================================================
	//==============================================================================================================


	public class TableSource : UITableViewSource
	{
		List<string> tableItems;
		string cellIdentifier = "TableCell";
		public string Voucher { get; set; }
		double price = 0;
		public event EventHandler<double> onCellSelectedForPrice;
		public event EventHandler<double> onCellDeselectedForPrice;
		public event EventHandler<int> onCellSelectedForVouchers;
		public event EventHandler<int> onCellDeselectedForVouchers;
		//


		public TableSource(List<string> items)
		{
			tableItems = items;
		}

		public List<string> ordersList = new List<string>();

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{

			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);


			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);

			}
			string[] split = tableItems[indexPath.Row].Split('#');
			string item = split[0];

			cell.TextLabel.Text = item;

			List<string> images = new List<string>() { "cappuccino.jpg", "Cappuccino1.jpg", "cappuccino2.jpg",
				"Cappuccino400.jpg", "CaramelFlan.jpg", "HazelnutCappuccino.jpg", "Unknown12.jpg","Pic1.jpg","Pic2.jpg",
				"Pic3.jpg","Pic4.jpg","Pic5.jpg","Pic6.jpg","Pic7.jpg","Pic8.jpg"};

			Random randomIndex = new Random();
			int index = randomIndex.Next(0, images.Count);
			UIImage image = UIImage.FromFile("TableImages/" + images[index]);
			cell.ImageView.Image = ResizeImage(image, 80, 80);




			return cell;
		}

		public UIImage ResizeImage(UIImage imageSource, float width, float height)
		{
			UIGraphics.BeginImageContext(new SizeF(width, height));
			imageSource.Draw(new RectangleF(0, 0, width, height));
			var imageResult = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return imageResult;
		}


		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Count;
		}



		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			ordersList.Add(tableItems[indexPath.Row]);
			string[] splitForPrice = tableItems[indexPath.Row].Split('#');
			price = double.Parse(splitForPrice[1]);
			string[] splitForVoucher = Voucher.Split(' ');
			int voucherNumber = int.Parse(splitForVoucher[0]);

			if (onCellSelectedForPrice != null)
			{
				onCellSelectedForPrice(tableView, price);

			}

			if (onCellSelectedForVouchers != null)
			{
				onCellSelectedForVouchers(tableView, voucherNumber);
			}
		}

		public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
		{

			string[] split = tableItems[indexPath.Row].Split('#');
			price = double.Parse(split[1]);

			string[] splitForVoucher = Voucher.Split(' ');
			int voucherNumber = int.Parse(splitForVoucher[0]);
			Debug.WriteLine(voucherNumber);
			if (onCellDeselectedForPrice != null)
			{
				onCellDeselectedForPrice(tableView, price);
			}

			ordersList.Remove(tableItems[indexPath.Row]);

			if (onCellDeselectedForVouchers != null)
			{
				onCellDeselectedForVouchers(tableView, voucherNumber);
			}

		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return "Order List\n";
		}


		public void GetOrdersListAndPriceFromDatabase(params string[] ordersAndPriceFromDatabase)
		{

			foreach (string elementOrders in ordersAndPriceFromDatabase)
			{
				tableItems.Add(elementOrders);
			}

		}

	}

	//==============================================================================================================
	//==============================================================================================================
	//==============================================================================================================
	//==============================================================================================================


	public class OrderWaitTime
	{

		const int AVERAGE_ORDER_TIME = 2;

		public int GetOrdersTotal { get; set; }

		public int CalculateWaitTime()
		{
			int time = GetOrdersTotal * AVERAGE_ORDER_TIME;
			return time;
		}

		public void UpdateToDB() { }
		public int GetTimeFromDB { get; set; }
	}

}

