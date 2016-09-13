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
