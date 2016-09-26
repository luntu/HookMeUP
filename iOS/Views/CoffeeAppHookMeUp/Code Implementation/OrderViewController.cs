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

		public string GetUserChannelName 
		{
			get; 
			set;
		}

		ParseObject TableNameOrders
		{
			get;
			set;
		}

		UITableViewCell Cell
		{
			get;
			set;
		}

		Coffee SelectedCoffeeItem
		{
			get;
			set;
		}

		NSIndexPath SelectedIndexPath
		{
			get;
			set;
		}

		int Badge = 0;

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

		string NamesOfTaggedOrders
		{
			get;
			set;
		} = "";

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
			ordersTable.Source = Source;
			ordersTable.ReloadData();

			Source.getSelectedCell += (sender, e) =>
			{
				Cell = e;
				SelectedIndexPath = ordersTable.IndexPathForCell(Cell);
				SelectedCoffeeItem = coffeeItems[SelectedIndexPath.Row];
			};

			Source.onCellForOrderName += (sender, e) =>
			{
				CellName = e;
			};

			Source.onCellSelectedForVouchers += (sender, e) =>
			{
				
				//var isAvailable = !VoucherCount.IsVoucherDepleted;
				//SelectedCoffeeItem.Available = isAvailable;

				//ordersTable.ReloadRows(new[] { SelectedIndexPath }, UITableViewRowAnimation.Fade);

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
				else
				if (NamesOfTaggedOrders.Equals(CellName))
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
				else if (VoucherCount.GetVoucher() !=0 && !VoucherCount.HasTag)
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
				showOrders += orderName + "\n";

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
						//submitt order

						TableNameOrders = new ParseObject("Orders");
						TableNameOrders["PersonOrdered"] = GetName;
						TableNameOrders["OrderList"] = items;
						TableNameOrders["Price"] = prices;
						TableNameOrders["IsOrderDone"] = false;
						TableNameOrders["Time"] = "" + time;
						CurrentUser["Vouchers"] = voucherUpdate;
						await TableNameOrders.SaveAsync();
						await CurrentUser.SaveAsync();
						items.Clear();

						//send push


						var push = new ParsePush();
						push.Channels = new string[] { "Admin" };
						push.Data = new Dictionary<string, object>
						{
							{"title","HookMeUp"},
							{"alert","New order from "+GetName.ToUpper()},
							{"badge",++Badge}

						};
						await push.SendAsync();
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

