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
	[Register ("RegisterViewController")]
	partial class RegisterViewController
	{
		[Outlet]
		UIKit.UIButton backButtonRegister { get; set; }

		[Outlet]
		UIKit.UITextField emailText { get; set; }

		[Outlet]
		UIKit.UITextField nameText { get; set; }

		[Outlet]
		UIKit.UITextField passwordTextR { get; set; }

		[Outlet]
		UIKit.UIScrollView registerUIScrollView { get; set; }

		[Outlet]
		UIKit.UIButton submitButton { get; set; }

		[Outlet]
		UIKit.UITextField surnameText { get; set; }

		[Outlet]
		UIKit.UITextField usernameTextR { get; set; }

		[Outlet]
		UIKit.UITextField verifyPasswordText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (backButtonRegister != null) {
				backButtonRegister.Dispose ();
				backButtonRegister = null;
			}

			if (emailText != null) {
				emailText.Dispose ();
				emailText = null;
			}

			if (nameText != null) {
				nameText.Dispose ();
				nameText = null;
			}

			if (passwordTextR != null) {
				passwordTextR.Dispose ();
				passwordTextR = null;
			}

			if (registerUIScrollView != null) {
				registerUIScrollView.Dispose ();
				registerUIScrollView = null;
			}

			if (submitButton != null) {
				submitButton.Dispose ();
				submitButton = null;
			}

			if (surnameText != null) {
				surnameText.Dispose ();
				surnameText = null;
			}

			if (usernameTextR != null) {
				usernameTextR.Dispose ();
				usernameTextR = null;
			}

			if (verifyPasswordText != null) {
				verifyPasswordText.Dispose ();
				verifyPasswordText = null;
			}
		}
	}
}
