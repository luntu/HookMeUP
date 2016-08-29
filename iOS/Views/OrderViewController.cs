using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Foundation;
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class OrderViewController : ScreenViewController
	{
		public TableSourceOrdering Source { get; set; }
		double dynamicPrice = 0.00;
		int voucherUpdate = 0;
		public int time;
		public List<string> items;
		public int detectVoucher = 0;


		public double getPrice;
		List<string> tableItems = new List<string> 
		{
			"Espresso#15,00", "Red Espresso#15.50", "Cappuccino#19.00","Red Cappuccino#19.50", "Vanilla Cappuccino#28.00", "Hazelnut Cappuccino#28.00",
			"Latte#22.50", "Red Latte#20.00", "Vanilla Latte#30.00", "Hazelnut Latte#30.00", "Cafe Americano#18.50", "Cafe Mocha#24.50", "Hot Chocolate#20.00"
		};

		OrderWaitTime orderWaitTime = new OrderWaitTime();

		public int GetVouchers { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			//inserting cells into the table

			VouchersLabel.Text = GetVouchers + " vouchers";
			Source = new TableSourceOrdering(tableItems);
			Source.Voucher = VouchersLabel.Text;
			ordersTable.Source = Source;
					
			Source.onCellSelectedForVouchers += (o, e) =>
			{
				e--;

				if (e == 0)  
				{
					VouchersLabel.Text = e + " Vouchers";
				}
				if (e < 0) 
				{
					detectVoucher++;
				}
				else 
				{
					
					VouchersLabel.Text = e + " Vouchers";
					Source.Voucher = VouchersLabel.Text;
				}
			};
			int s = 0;
			Source.onCellDeselectedForVouchers += (o, e) =>
			{


				if (detectVoucher == 0) // if vouchers are not negative because detect vouchers increment negatively.
				{
					e++;
					VouchersLabel.Text = e + " Vouchers";
					Source.Voucher = VouchersLabel.Text;
					ordersTable.BackgroundColor = UIColor.Clear;

				}

				else 
				{
					s = detectVoucher;
					detectVoucher--; //detect voucher is > 0, meaning vouchers are negative. so reduce it until it hits zero to increase the vouchers


				}
			};



			Source.onCellSelectedForPrice += (o, e) =>
			{
				
				if (detectVoucher != 0)
				{
					dynamicPrice += e;
					costText.Text = dynamicPrice.ToString("R 0.00");
				}


			};

			Source.onCellDeselectedForPrice += (o, e) =>
			{


				if (detectVoucher == 0 && s == 1) 
				{
			
					dynamicPrice -= e;
					costText.Text = "R 0,00";
				}

				if (detectVoucher < 0 || detectVoucher != 0)
				{
					dynamicPrice -= e;
					costText.Text = dynamicPrice.ToString("R 0.00");
				}
			
			};
		
			hookMeUPButton.TouchUpInside += (obj, evt) =>
			{
				// getting orders

				try
				{
					
					if (Source.ordersList != null && !Source.ordersList[0].Equals(""))
					{
						Order();

					}
					else AlertPopUp("Error", "No order(s) selected", "OK");

				}
				catch (ArgumentOutOfRangeException)
				{
					AlertPopUp("Error", "No order(s) selected", "OK");
				}

			};

			viewOrderButton.TouchUpInside +=(o,e) =>
			{
				NavigationScreenController(queueViewController);
			};

		}



		public string GetName 
		{
			get;
			set;
		} 
		
		public ParseObject CurrentUser 
		{
			get;
			set;
		}

		void Order()
		{

			items = new List<string>();

			double prices = 0;
			string elementShow = "";

			foreach (string orderElements in Source.ordersList)
			{
				
				string[] splitElements = orderElements.Split('#');
				elementShow += splitElements[0] + "\n";
				items.Add(splitElements[0]);
				prices += double.Parse(Source.FormatPrice(splitElements[1]));

			}

			UIAlertView alert = new UIAlertView();
			alert.Title = "Orders selected";
			alert.Message = elementShow.Trim();
			alert.AddButton("Order");
			alert.AddButton("Cancel");

			alert.Clicked += async (o, e) =>
		    {
			   if (e.ButtonIndex == 0)
			   {
				   //submit datadase. Notify Vuyo

				   orderWaitTime.GetOrdersTotal = Source.ordersList.Count;
				   time = orderWaitTime.CalculateWaitTime();
				   AlertPopUp("Order on the way", "Your order will take about " + time + " minutes", "OK");

				   string[] arrSplit = VouchersLabel.Text.Split(' ');
				   voucherUpdate = int.Parse(arrSplit[0]);


				   loadingOverlay = new LoadingOverlay(bounds);
				   View.Add(loadingOverlay);
				  
				   try
				   {

					   tableNameOrders["PersonOrdered"] = GetName;
					   tableNameOrders["OrderList"] = items;
					   tableNameOrders["Price"] = prices;
					   tableNameOrders["IsOrderDone"] = false;
					   tableNameOrders["Time"] = ""+time;
					   CurrentUser["Vouchers"] = voucherUpdate;
					   await tableNameOrders.SaveAsync();
					   await CurrentUser.SaveAsync();

				   }
				   catch (ParseException q)
				   {
					  Debug.WriteLine(q.StackTrace);
				   }

				   loadingOverlay.Hide();
				}

		    };
			alert.Show();

		}
			
	}

	//==================================================================================================================
	//==================================================================================================================
	//==================================================================================================================


	public class TableSourceOrdering : UITableViewSource
	{
		List<string> tableItems;
		string cellIdentifier = "TableCell";
		public string Voucher { get; set; }
		double price = 0;
		public event EventHandler<double> onCellSelectedForPrice;
		public event EventHandler<double> onCellDeselectedForPrice;
		public event EventHandler<int> onCellSelectedForVouchers;
		public event EventHandler<int> onCellDeselectedForVouchers;

		public List<string> ordersList = new List<string>();


		public TableSourceOrdering(List<string> items)
		{
			tableItems = items;
		}

		public int NumberOfItemsForCash 
		{
			get;
			set;
		}

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

			List<string> images = new List<string>
			{
				"cappuccino.jpg", "Cappuccino1.jpg", "cappuccino2.jpg",
				"Cappuccino400.jpg", "CaramelFlan.jpg", "HazelnutCappuccino.jpg", "Unknown12.jpg","Pic1.jpg","Pic2.jpg",
				"Pic3.jpg","Pic4.jpg","Pic5.jpg","Pic6.jpg","Pic7.jpg","Pic8.jpg"
			};

			Random randomIndex = new Random();
			int index = randomIndex.Next(0, images.Count);
			UIImage image = UIImage.FromFile("TableImages/" + images[index]);
			cell.ImageView.Image = ResizeImage(image, 80, 80);

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			ordersList.Add(tableItems[indexPath.Row]);

			string[] splitForPrice = tableItems[indexPath.Row].Split('#');

			string priceAmount = FormatPrice(splitForPrice[1]);
					
			price = double.Parse(priceAmount);

			string[] splitForVoucher = Voucher.Split(' ');
			int voucherNumber = int.Parse(splitForVoucher[0]);

			if (onCellSelectedForVouchers != null)
			{
				onCellSelectedForVouchers(tableView, voucherNumber);
			}

			if (onCellSelectedForPrice != null)
			{
				onCellSelectedForPrice(tableView, price);

			}

		}

		public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
		{

			string[] split = tableItems[indexPath.Row].Split('#');

			string priceAmount = FormatPrice(split[1]);
			price = double.Parse(priceAmount);
		
			string[] splitForVoucher = Voucher.Split(' ');
			int voucherNumber = int.Parse(splitForVoucher[0]);
		
			if (onCellDeselectedForVouchers != null)
			{
				onCellDeselectedForVouchers(tableView, voucherNumber);
			}

			if (onCellDeselectedForPrice != null)
			{
				onCellDeselectedForPrice(tableView, price);
			}

			ordersList.Remove(tableItems[indexPath.Row]);

		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return "Order List\n";
		}

		public string FormatPrice(string s) 
		{
			return s.Replace(".", ",");
		}

		UIImage ResizeImage(UIImage imageSource, float width, float height)
		{
			UIGraphics.BeginImageContext(new SizeF(width, height));
			imageSource.Draw(new RectangleF(0, 0, width, height));
			var imageResult = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return imageResult;
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

	}

}

