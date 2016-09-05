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
		UIKit.UIButton backButtonForgot { get; set; }

		[Outlet]
		UIKit.UITextField emailTextForgot { get; set; }

		[Outlet]
		UIKit.UIScrollView forgotUIScrollView { get; set; }

		[Outlet]
		UIKit.UIButton getPasswordButton { get; set; }

		[Outlet]
		UIKit.UITextField usernameTextForgot { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (backButtonForgot != null) {
				backButtonForgot.Dispose ();
				backButtonForgot = null;
			}

			if (emailTextForgot != null) {
				emailTextForgot.Dispose ();
				emailTextForgot = null;
			}

			if (forgotUIScrollView != null) {
				forgotUIScrollView.Dispose ();
				forgotUIScrollView = null;
			}

			if (getPasswordButton != null) {
				getPasswordButton.Dispose ();
				getPasswordButton = null;
			}

			if (usernameTextForgot != null) {
				usernameTextForgot.Dispose ();
				usernameTextForgot = null;
			}
		}
	}
}
