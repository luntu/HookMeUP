// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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

            if (viewOrderButton != null) {
                viewOrderButton.Dispose ();
                viewOrderButton = null;
            }

            if (VouchersLabel != null) {
                VouchersLabel.Dispose ();
                VouchersLabel = null;
            }
        }
    }
}