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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIButton forgotPasswordButton { get; set; }

		[Outlet]
		UIKit.UIButton loginButton { get; set; }

		[Outlet]
		UIKit.UIScrollView ParentScrollView { get; set; }

		[Outlet]
		UIKit.UITextField passwordText { get; set; }

		[Outlet]
		UIKit.UIButton registerButton { get; set; }

		[Outlet]
		UIKit.UITextField usernameText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (forgotPasswordButton != null) {
				forgotPasswordButton.Dispose ();
				forgotPasswordButton = null;
			}

			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}

			if (passwordText != null) {
				passwordText.Dispose ();
				passwordText = null;
			}

			if (registerButton != null) {
				registerButton.Dispose ();
				registerButton = null;
			}

			if (usernameText != null) {
				usernameText.Dispose ();
				usernameText = null;
			}

			if (ParentScrollView != null) {
				ParentScrollView.Dispose ();
				ParentScrollView = null;
			}
		}
	}
}
