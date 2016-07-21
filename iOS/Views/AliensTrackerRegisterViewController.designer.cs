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
	[Register ("AliensTrackerRegisterViewController")]
	partial class AliensTrackerRegisterViewController
	{
		[Outlet]
		UIKit.UIButton backFireButton { get; set; }

		[Outlet]
		UIKit.UITextField contactNoFireText { get; set; }

		[Outlet]
		UIKit.UITextField nameFireText { get; set; }

		[Outlet]
		UIKit.UIButton saveFireButton { get; set; }

		[Outlet]
		UIKit.UITextField surnameFireText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nameFireText != null) {
				nameFireText.Dispose ();
				nameFireText = null;
			}

			if (surnameFireText != null) {
				surnameFireText.Dispose ();
				surnameFireText = null;
			}

			if (contactNoFireText != null) {
				contactNoFireText.Dispose ();
				contactNoFireText = null;
			}

			if (saveFireButton != null) {
				saveFireButton.Dispose ();
				saveFireButton = null;
			}

			if (backFireButton != null) {
				backFireButton.Dispose ();
				backFireButton = null;
			}
		}
	}
}
