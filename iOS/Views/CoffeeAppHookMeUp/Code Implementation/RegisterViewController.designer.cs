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