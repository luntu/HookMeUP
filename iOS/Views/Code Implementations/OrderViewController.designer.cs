// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace HookMeUP.iOS
{
	[Register ("OrderViewController")]
	partial class OrderViewController
	{
		[Outlet]
		UIKit.UITextField costText { get; set; }

		[Outlet]
		UIKit.UIButton hookMeUPButton { get; set; }

		[Outlet]
		UIKit.UITableView ordersTable { get; set; }

		[Outlet]
		UIKit.UIButton viewOrderButton { get; set; }

		[Outlet]
		UIKit.UILabel VouchersLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (costText != null) {
				costText.Dispose ();
				costText = null;
			}

			if (hookMeUPButton != null) {
				hookMeUPButton.Dispose ();
				hookMeUPButton = null;
			}

			if (ordersTable != null) {
				ordersTable.Dispose ();
				ordersTable = null;
			}

			if (VouchersLabel != null) {
				VouchersLabel.Dispose ();
				VouchersLabel = null;
			}

			if (viewOrderButton != null) {
				viewOrderButton.Dispose ();
				viewOrderButton = null;
			}
		}
	}
}
