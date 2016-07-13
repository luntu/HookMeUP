using System;

using UIKit;

namespace HookMeUP.iOS
{
	public partial class ScreenViewController : UIViewController
	{
		
		public static RegisterViewController registerViewController = new RegisterViewController();
		public static LoginViewController loginViewController = new LoginViewController();
		public static ForgotPasswordViewController forgotPasswordViewController = new ForgotPasswordViewController();

		public void AlertPopUp(string title, string message, params string[] buttonText)
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = title;
			alert.Message = message;

			foreach (string elements in buttonText)
			{
				alert.AddButton(elements);
			}

			alert.Show();

		}

		public void BorderButton(params UIButton[] button)
		{
			foreach (UIButton element in button)
			{
				element.Layer.BorderWidth = 1;
				element.Layer.CornerRadius = 4;
				element.Layer.BorderColor = UIColor.Cyan.CGColor;

			}

		}

		public void NavigationScreenController(UIViewController screen)
		{
			NavigationController.PushViewController(screen, true);

		}

	}
}

