using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
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
		public event EventHandler<string> onCellForOrderName;

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

			if (onCellForOrderName != null)
				onCellForOrderName(tableView, coffeeItem.Title);
			

			if (onCellSelectedForVouchers != null)
				onCellSelectedForVouchers(tableView, voucherNumber);
			

			if (onCellSelectedForPrice != null)
				onCellSelectedForPrice(tableView, price);
			
		}

		public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
		{

			Coffee coffeeItem = coffeeItems[indexPath.Row];

			price = double.Parse(FormatPrice(coffeeItem.Price));

			string[] splitForVoucher = Voucher.Split(' ');
			int voucherNumber = int.Parse(splitForVoucher[0]);

			if (onCellForOrderName != null)
				onCellForOrderName(tableView, coffeeItem.Title);

			if (onCellDeselectedForVouchers != null)
				onCellDeselectedForVouchers(tableView, voucherNumber);
			

			if (onCellDeselectedForPrice != null)
				onCellDeselectedForPrice(tableView, price);
			

			ordersList.Remove(coffeeItem);

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
}

