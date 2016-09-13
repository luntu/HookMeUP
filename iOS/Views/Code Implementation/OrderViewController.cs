using System;
using System.Collections.Generic;
using System.Diagnostics;
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

}

