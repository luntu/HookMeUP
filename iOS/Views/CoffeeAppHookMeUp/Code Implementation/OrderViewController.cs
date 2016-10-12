using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public ParseUser CurrentUser
		{
			get;
			set;
		}

		ParseObject TableNameOrders
		{
			get;
			set;
		}
			
		NSIndexPath SelectedIndexPath
		{
			get;
			set;
		}

		int voucherUpdate = 0;
		public int time;

		List<string> items = new List<string>();
		List<Coffee> coffeeItems = new List<Coffee>();
		List<TagOrder> taggedOrders = new List<TagOrder>();
		List<string> taggedOrderNamesForPrice = new List<string>();
		OrderWaitTime orderWaitTime = new OrderWaitTime();
		VoucherCount VoucherCount = new VoucherCount();
		PriceCount PriceCount = new PriceCount();


		public double getPrice;
		string showOrders = "";

		public string GetUserChannelName
		{
			get;
			set;
		}

		public int GetVouchers
		{
			get;
			set;
		}

		string CellName
		{
			get;
			set;
		}

		public string GetName
		{
			get;
			set;
		}

		public string GetSurname
		{
			get;
			set;
		}

		string NamesOfTaggedOrders
		{
			get;
			set;
		}

		bool DidInitiliseSource 
		{
			get;
			set;
		}

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
			if(DidInitiliseSource)
			{
				ResetVoucher();
				VouchersLabel.Text = Source.GetVoucher() + " vouchers";
			}
			ResetScreen();
		}

		void ResetVoucher() 
		{ 
			Source.ResetVoucher();
		}

		void SetupView()
		{
			VouchersLabel.Text = GetVouchers + " vouchers";

			hookMeUPButton.TouchUpInside += (obj, evt) =>
			{
				// getting orders
				try
				{
					if (Source.ordersList != null && !Source.ordersList[0].Equals("")) Order();
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
			DidInitiliseSource = false;
			try
			{
				loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				View.Add(loadingOverlay);
				ParseQuery<ParseObject> query = ParseObject.GetQuery("Coffees");
				query.Include("Title").Include("Price").Include("ImageName");

				var iEnumerableColl = await query.FindAsync();

				foreach (ParseObject coffeeElements in iEnumerableColl)
				{
					string coffeeName = coffeeElements.Get<string>("Title");
					string imageName = coffeeElements.Get<string>("ImageName");
					double price = coffeeElements.Get<double>("Price");

					coffeeItems.Add(new Coffee(coffeeName, imageName, "" + price));

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

			Source.Launched = true;
			Source.Ordered = false;
			Source.GetInitialVoucher = VouchersLabel.Text;

			ordersTable.Source = Source;
			ordersTable.ReloadData();
			DidInitiliseSource = true;

			Source.onCellForOrderName += (sender, e) =>
			{
				CellName = e;
			};

			Source.onCellSelectedForVouchers += (sender, e) =>
			{    
					VoucherCount.Voucher = e;
					VoucherCount.IsSelected = true;
					VoucherCount.IsDeselected = false;
					VoucherCount.VoucherChange();
					VouchersLabel.Text = "" + VoucherCount.GetVoucher() + " Vouchers";
					Source.Voucher = VouchersLabel.Text;

					if (!VoucherCount.IsVoucherDepleted)
					{  // tag all the orders purchased by vouchers
						taggedOrders.Add(new TagOrder(CellName));
						taggedOrderNamesForPrice.Add(CellName);
					}
				
			};

			Source.onCellDeselectedForVouchers += (sender, e) =>
			{
				//increments voucher if its a tagged order

				VoucherCount.HasTag = false;

				foreach (TagOrder order in taggedOrders)
				{
					//if (!NamesOfTaggedOrders.Contains(order.OrderName)) NamesOfTaggedOrders += order.OrderName + "*";
					if (order.OrderName.Equals(CellName))
					{
						Debug.WriteLine(order.OrderName);
						NamesOfTaggedOrders = order.OrderName;
						taggedOrders.Remove(order);
						VoucherCount.HasTag = true;
						break;
					}

				}

				if (PriceCount.Depleted)
				{
					VoucherCount.IsDeselected = true;
					VoucherCount.IsSelected = false;
					VoucherCount.VoucherChange();
					VouchersLabel.Text = "" + VoucherCount.GetVoucher() + " Vouchers";
					Source.Voucher = VouchersLabel.Text;
				}
				else if (CellName.Equals(NamesOfTaggedOrders))
				{
					VoucherCount.IsDeselected = true;
					VoucherCount.IsSelected = false;
					VoucherCount.VoucherChange();
					VouchersLabel.Text = "" + VoucherCount.GetVoucher() + " Vouchers";
					Source.Voucher = VouchersLabel.Text;
				}
				NamesOfTaggedOrders = string.Empty;

			};

			Source.onCellSelectedForPrice += (sender, e) =>
			{
				if (VoucherCount.IsVoucherDepleted)
				{
					PriceCount.Price = e;
					PriceCount.Selected = true;
					PriceCount.Deselected = false;
					PriceCount.PriceChange();
					costText.Text = PriceCount.GetPrice().ToString("R 0.00");
				}

			};

			Source.onCellDeselectedForPrice += (sender, e) =>
			{
				const int VOUCHER_BEFORE_EXECUTION_TO_ZERO = 1;

				if (VoucherCount.IsVoucherDepleted && VoucherCount.Voucher != VOUCHER_BEFORE_EXECUTION_TO_ZERO)
				{
					PriceCount.Price = e;
					PriceCount.Selected = false;
					PriceCount.Deselected = true;
					PriceCount.PriceChange();
					costText.Text = PriceCount.GetPrice().ToString("R 0.00");
				}
				else if (VoucherCount.GetVoucher() != 0 && !VoucherCount.HasTag)
				{
					PriceCount.Price = e;
					PriceCount.Selected = false;
					PriceCount.Deselected = true;
					PriceCount.PriceChange();
					costText.Text = PriceCount.GetPrice().ToString("R 0.00");
				}

			};


		}

		void ResetScreen()
		{
			ResetTableView();
			try
			{
				taggedOrders.Clear();
				Source.ordersList.Clear();
				costText.Text = "R 0,00";
				PriceCount.ResetPrice();
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
			if (costText.Text.Equals("R 0,00")) OrderAnyway();
			else OrderWithPrice();
		}

		void OrderAnyway()
		{
			double prices = 0;
			foreach (Coffee orderElements in Source.ordersList)
			{
				string orderName = orderElements.Title;
				showOrders += orderName + "\n";

				items.Add(orderName);
				//prices += double.Parse(Source.FormatPrice(orderElements.Price));
			}
			prices = double.Parse(costText.Text.Split(' ')[1]);

			UIAlertView alert = PopUp("Orders selected", showOrders.Trim(), "Order", "Cancel");
			showOrders = string.Empty;

			alert.Clicked += (o, e) =>
		   	{
			   if (e.ButtonIndex == 0) SubmittOrders(prices);
			   else items.Clear();
		   	};
			alert.Show();
		}

		void OrderWithPrice()
		{
			double price = 0;

			price = double.Parse(costText.Text.Split(' ')[1]);
			
			UIAlertView alert = PopUp("Cost", "Are you gonna pay "+price.ToString("R 0.00"), "Yes", "No");

			alert.Clicked += (o, e) =>
		   	{	
				if (e.ButtonIndex == 0) OrderAnyway();
			   	else items.Clear();
		 	};
			alert.Show();
		}

		UIAlertView PopUp(string title, string message, params string[] buttons)
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = title;
			alert.Message = message;
			foreach (string buttonElements in buttons)
				alert.AddButton(buttonElements);
			return alert;
		}

		async void SubmittOrders(double prices)
		{
			orderWaitTime.GetOrdersTotal = Source.ordersList.Count;
			time = orderWaitTime.CalculateWaitTime();
			AlertPopUp("Order on the way", "Your order will take about " + time + " minutes", "OK");
			string[] arrSplit = VouchersLabel.Text.Split(' ');
			voucherUpdate = int.Parse(arrSplit[0]);

			loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(loadingOverlay);

			try
			{
				TableNameOrders = new ParseObject("Orders");
				TableNameOrders["PersonOrdered"] = GetName;
				TableNameOrders["OrderList"] = items;
				TableNameOrders["Price"] = prices;
				TableNameOrders["IsOrderDone"] = false;
				TableNameOrders["OrderReceivedByAdmin"] = false;
				TableNameOrders["Time"] = "" + time;
				TableNameOrders["UserChannel"] = GetUserChannelName;
				CurrentUser["Vouchers"] = voucherUpdate;

				await TableNameOrders.SaveAsync();
				await CurrentUser.SaveAsync();
				items.Clear();

				//send push

				var push = new ParsePush();
				push.Channels = new List<string> { "Admin" };
			
				push.Data = new Dictionary<string, object>
				{
					{"alert", "New order from " + (GetName + " " + GetSurname).ToUpper()},
					{"sound","default"},
					{"badge","Increment"}
				};

				await push.SendAsync();

				Source.Ordered = true;
				Source.GetInitialVoucher = voucherUpdate + " vouchers";

			}
			catch (ParseException q)
			{
				Debug.WriteLine(q.StackTrace);
			}

			ResetScreen();
			loadingOverlay.Hide();
		}

	}

}

