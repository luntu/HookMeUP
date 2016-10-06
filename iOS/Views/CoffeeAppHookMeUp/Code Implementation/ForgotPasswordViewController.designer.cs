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
        UIKit.UITextField emailTextForgot { get; set; }


        [Outlet]
        UIKit.UIScrollView forgotUIScrollView { get; set; }


        [Outlet]
        UIKit.UIButton getPasswordButton { get; set; }

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
        }
    }
}