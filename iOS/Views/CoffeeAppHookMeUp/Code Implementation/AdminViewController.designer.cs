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
    [Register ("AdminViewController")]
    partial class AdminViewController
    {
        [Outlet]
        UIKit.UITableView AminOrdersTable { get; set; }


        [Outlet]
        UIKit.UIButton viewUnpaidButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AminOrdersTable != null) {
                AminOrdersTable.Dispose ();
                AminOrdersTable = null;
            }

            if (viewUnpaidButton != null) {
                viewUnpaidButton.Dispose ();
                viewUnpaidButton = null;
            }
        }
    }
}