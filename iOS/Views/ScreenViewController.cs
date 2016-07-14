using System;

using UIKit;

namespace HookMeUP.iOS
{
	public partial class ScreenViewController : UIViewController
	{
		
		public static RegisterViewController registerViewController = new RegisterViewController();
		public static LoginViewController loginViewController = new LoginViewController();
		public static ForgotPasswordViewController forgotPasswordViewController = new ForgotPasswordViewController();
		public static OrderViewController orderViewController = new OrderViewController();

		public void AlertPopUp(string title, string message, params string[] buttonText)
		{

			if (!title.Equals("") && !message.Equals("") && buttonText != null) { 

				UIAlertView alert = new UIAlertView();
				alert.Title = title;
				alert.Message = message;

				foreach (string elements in buttonText)
				{
					alert.AddButton(elements);
				}

				alert.Show();
			}



		}

		public void BorderButton(params UIButton[] button)
		{
			if(button != null){ 

				foreach (UIButton element in button)
				{
					element.Layer.BorderWidth = 1;
					element.Layer.CornerRadius = 4;
					element.Layer.BorderColor = UIColor.Cyan.CGColor;

				}
			
			}


		}

		public void NavigationScreenController(UIViewController screen)
		{
			NavigationController.PushViewController(screen, true);

		}

		public void ClearFields(params UITextField[] texts) {

			if (texts != null)
			{
				foreach (UITextField parameters in texts)
				{
					parameters.Text = "";
				}
			}
		}

	}
}

