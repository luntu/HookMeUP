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
		public TableSourceOrdering Source 
		{ 
			get;
			private set;
		}
		VoucherCount VoucherCount = new VoucherCount();

		PriceCount PriceCount 
		{
			get;
			set;
		}

		//double dynamicPrice = 0.00;
		int voucherUpdate = 0;
		public int time;
		List<string> items = new List<string>();
		List<Coffee> coffeeItems = new List<Coffee>();
		ParseObject tableNameOrders;

		public int detectVoucher = 0;
		public double getPrice;
		string showOrders = "";

		OrderWaitTime orderWaitTime = new OrderWaitTime();

		public int GetVouchers { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetupView();
			LoadTableFuctionality();
			costText.Text = "R 0,00";
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ResetScreen();

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


		void SetupView()
		{
			VouchersLabel.Text = GetVouchers + " vouchers";
			PriceCount = new PriceCount(costText);
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

			viewOrderButton.TouchUpInside += (o, e) =>
			 {
				 NavigationScreenController(queueViewController);
			 };
		}

		async void LoadTableFuctionality()
		{
			try
			{
				loadingOverlay = new LoadingOverlay(bounds);
				View.Add(loadingOverlay);
				ParseQuery<ParseObject> query = ParseObject.GetQuery("Coffees");
				query.Include("Title").Include("Price").Include("ImageName");

				var iEnumerableColl = await query.FindAsync();

				foreach (ParseObject coffeeElements in iEnumerableColl) 
				{
					string coffeeName = coffeeElements.Get<string>("Title");
					string imageName = coffeeElements.Get<string>("ImageName");
					double price = coffeeElements.Get<double>("Price");

					coffeeItems.Add(new Coffee(coffeeName,imageName,""+price));
				
				}

				loadingOverlay.Hide();

			}
			catch (ParseException)
			{
				loadingOverlay.Hide();
				AlertPopUp("Error", "Connection error", "OK");
			}

			Source = new TableSourceOrdering(coffeeItems);
			Source.Voucher = VouchersLabel.Text;
			ordersTable.Source = Source;
			ordersTable.ReloadData();

			Source.onCellSelectedForVouchers += (sender, e) => 
			{
				VoucherCount.Voucher = e;
				VoucherCount.IsSelected = true;
				VoucherCount.IsDeselected = false;
				DisplayAndSaveVouchers();
			};

			Source.onCellDeselectedForVouchers += (sender, e) =>
			{
				VoucherCount.Voucher = e;
				VoucherCount.IsDeselected = true;
				VoucherCount.IsSelected = false;
				DisplayAndSaveVouchers();
			};

			Source.onCellSelectedForPrice += (sender, e) =>
			{
				PriceCount.Price = e;
				PriceCount.Selected = true;
				PriceCount.Deselected = false;
				//PriceCount

			};


		}
		void DisplayAndSaveVouchers() 
		{
			VoucherCount.VoucherChange();
			VouchersLabel.Text = "" + VoucherCount.GetVoucher() + " Vouchers";
			Source.Voucher = VouchersLabel.Text;
		}

		void ResetScreen() 
		{
			ResetTableView();
			try
			{
				Source.ordersList.Clear();
				costText.Text = "R 0,00";
			}
			catch (NullReferenceException ex)
			{
				Debug.WriteLine(ex.Source);
			}
		}

		void ResetTableView()
		{
			ordersTable.ReloadData();
			items.Clear();
		}

		void Order()
		{
			double prices = 0;


			foreach (Coffee orderElements in Source.ordersList)
			{
				string orderName = orderElements.Title;
				showOrders += orderName+"\n";

				items.Add(orderName);
				prices = double.Parse(Source.FormatPrice(orderElements.Price));
			
			}

			UIAlertView alert = new UIAlertView();
			alert.Title = "Orders selected";
			alert.Message = showOrders.Trim();
			showOrders = string.Empty;
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
					   tableNameOrders = new ParseObject("Orders");
					   tableNameOrders["PersonOrdered"] = GetName;
					   tableNameOrders["OrderList"] = items;
					   tableNameOrders["Price"] = prices;
					   tableNameOrders["IsOrderDone"] = false;
					   tableNameOrders["Time"] = ""+time;
					   CurrentUser["Vouchers"] = voucherUpdate;
					   await tableNameOrders.SaveAsync();
					   await CurrentUser.SaveAsync();
						items.Clear();
				   }
				   catch (ParseException q)
				   {
					  Debug.WriteLine(q.StackTrace);
				   }
					ResetScreen();
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
		//List<string> tableItems;
		List<Coffee> coffeeItems;
		string cellIdentifier = "TableCell";
		public string Voucher { get; set; }
		double price = 0;
		public event EventHandler<double> onCellSelectedForPrice;
		public event EventHandler<double> onCellDeselectedForPrice;
		public event EventHandler<int> onCellSelectedForVouchers;
		public event EventHandler<int> onCellDeselectedForVouchers;

		public List<Coffee> ordersList = new List<Coffee>();

		public TableSourceOrdering(List<Coffee> items)
		{
			coffeeItems = items;
		}

		public int NumberOfItemsForCash 
		{
			get;
			set;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			Coffee item = coffeeItems[indexPath.Row];

			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
			}

			cell.TextLabel.Text = item.Title;
			var directory = "TableImages/";
			UIImage image = UIImage.FromFile(directory + item.ImageName);
			cell.ImageView.Image = ResizeImage(image, 80, 80);

			return cell;

		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			//return tableItems.Count;
			return coffeeItems.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Coffee coffeeItem = coffeeItems[indexPath.Row];
			
			ordersList.Add(coffeeItem);
			
			price = double.Parse(FormatPrice(coffeeItem.Price));
			
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

			Coffee coffeItem = coffeeItems[indexPath.Row];
			
			price = double.Parse(FormatPrice(coffeItem.Price));
			
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

			ordersList.Remove(coffeItem);

		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return "Order List\n";
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 60f;
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


	class OrderWaitTime
	{

		const int AVERAGE_ORDER_TIME = 2;

		public int GetOrdersTotal { get; set; }

		public int CalculateWaitTime()
		{
			int time = GetOrdersTotal * AVERAGE_ORDER_TIME;
			return time;
		}

	}

	//==============================================================================================================
	//==============================================================================================================
	//==============================================================================================================
	//==============================================================================================================

	class VoucherCount
	{

		bool IsVoucherDepleted 
		{
			get;
			set;
		}
		bool IsVoucherNegative 
		{ 
			get;
			set;
		}

		public int Voucher
		{
			get;
			set;
		}

		public double Price
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public bool IsDeselected
		{
			get;
			set;
		}

		public void VoucherChange()
		{
			if (Voucher > 0)
			{
				if (IsSelected) --Voucher;
				if (IsDeselected) ++Voucher;

			}
			else if (Voucher == 0) IsVoucherDepleted = true;
			else
			{
				IsVoucherNegative = true;

			}
		}

		public int GetVoucher()
		{
			return Voucher;
		}
	}

	//==================================================================================================================
	//==================================================================================================================
	//==================================================================================================================
	//==================================================================================================================

	class PriceCount
	{
		public double Price
		{
			get;
			set;
		}

		public bool Selected 
		{
			get;
			set;
		}

		public bool Deselected
		{
			get;
			set;
		}

		bool Depleted 
		{
			get;
			set;
		}

		bool Negative
		{
			get;
			set;
		}

		int NegativeVouchers
		{ 
			get;
			set;
		}
		UITextField CostText 
		{
			get;
			set;
		}

		public PriceCount(UITextField costText) 
		{
			CostText = costText;
		}
			
	}

}

