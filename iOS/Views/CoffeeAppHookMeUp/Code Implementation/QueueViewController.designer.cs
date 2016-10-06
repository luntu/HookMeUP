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
    [Register ("QueueViewController")]
    partial class QueueViewController
    {
        [Outlet]
        UIKit.UITableView ActiveOrdersTable { get; set; }


        [Outlet]
        UIKit.UIButton backOrdersButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActiveOrdersTable != null) {
                ActiveOrdersTable.Dispose ();
                ActiveOrdersTable = null;
            }

            if (backOrdersButton != null) {
                backOrdersButton.Dispose ();
                backOrdersButton = null;
            }
        }
    }
}