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
    [Register ("ForgotPasswordViewController")]
    partial class ForgotPasswordViewController
    {
        [Outlet]
        UIKit.UIButton backButtonForgot { get; set; }


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
            if (backButtonForgot != null) {
                backButtonForgot.Dispose ();
                backButtonForgot = null;
            }

            if (employeeNoForgot != null) {
                employeeNoForgot.Dispose ();
                employeeNoForgot = null;
            }

            if (getPasswordButton != null) {
                getPasswordButton.Dispose ();
                getPasswordButton = null;
            }

            if (getPasswordText != null) {
                getPasswordText.Dispose ();
                getPasswordText = null;
            }

            if (usernameTextForgot != null) {
                usernameTextForgot.Dispose ();
                usernameTextForgot = null;
            }
        }
    }
}