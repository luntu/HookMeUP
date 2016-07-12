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
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        UIKit.UIButton forgotPasswordButton { get; set; }


        [Outlet]
        UIKit.UIButton loginButton { get; set; }


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
        }
    }
}