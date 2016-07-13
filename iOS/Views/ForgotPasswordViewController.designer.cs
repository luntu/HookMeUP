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
	[Register ("ForgotPasswordViewController")]
	partial class ForgotPasswordViewController
	{
		[Outlet]
		UIKit.UITextField employeeNoForgot { get; set; }

		[Outlet]
		UIKit.UIButton getPasswordButton { get; set; }

		[Outlet]
		UIKit.UITextField getPasswordText { get; set; }

		[Outlet]
		UIKit.UITextField usernameTextForgot { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (employeeNoForgot != null) {
				employeeNoForgot.Dispose ();
				employeeNoForgot = null;
			}

			if (usernameTextForgot != null) {
				usernameTextForgot.Dispose ();
				usernameTextForgot = null;
			}

			if (getPasswordText != null) {
				getPasswordText.Dispose ();
				getPasswordText = null;
			}

			if (getPasswordButton != null) {
				getPasswordButton.Dispose ();
				getPasswordButton = null;
			}
		}
	}
}
