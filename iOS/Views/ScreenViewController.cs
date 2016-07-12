using System;

using UIKit;

namespace HookMeUP.iOS
{
	public partial class ScreenViewController : UIViewController
	{
		public static  RegisterViewController registerViewController = new RegisterViewController();

		public static LoginViewController loginViewController = new LoginViewController();

		public void alertPopUp(string title, string message, params string[] buttonText) {
			UIAlertView alert = new UIAlertView();
			alert.Title = title;
			alert.Message = message;

				
		}

	}

		
	
}


