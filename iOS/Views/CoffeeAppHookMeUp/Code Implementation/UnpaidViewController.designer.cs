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
	[Register ("UnpaidViewController")]
	partial class UnpaidViewController
	{
		[Outlet]
		UIKit.UIButton backButton { get; set; }

		[Outlet]
		UIKit.UITableView unpaidTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (unpaidTable != null) {
				unpaidTable.Dispose ();
				unpaidTable = null;
			}

			if (backButton != null) {
				backButton.Dispose ();
				backButton = null;
			}
		}
	}
}
