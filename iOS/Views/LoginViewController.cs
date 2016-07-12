using System;
using System.Diagnostics;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class LoginViewController : ScreenViewController
	{

	

		//static RegisterViewController registerViewController = new RegisterViewController();
		UIAlertView alert = new UIAlertView();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			  
			forgotPasswordButton.TouchUpInside += (o, e) => { /*forgot password page*/ 

			};

			int i = 0;

			loginButton.TouchUpInside += (o, e) => {
				i++;
				string[] split = registerViewController.getValues().Split('#');

				string usernameR = split[2];
				string usernameL = usernameText.Text;

				string passwordR = split[3];
				string passwordL = passwordText.Text;

				try{
					if (usernameL.Equals(usernameR) && passwordL.Equals(passwordR))
					{
						NavigationController.PushViewController(registerViewController, true);
					}
					else {

						alert.Title = "Login failed";
						alert.Message = "Username and/or password is incorrect";
						alert.AddButton("OK");
						alert.Show();
					}
				}catch (System.ArgumentOutOfRangeException) {
					alert.Title = "Login failed";
					alert.Message = "Username and/or password is incorrect";
					alert.AddButton("OK");
					alert.Show();
				}

				if (i == 3) {

					alert.Title = "Login failed";
					alert.Message = "You have entered the password 3 times incorrectly\n we suggest you click the 'forgot password button' or 'Register button'";
					alert.AddButton("OK");
					alert.Show();
					//forgotPasswordButton

					forgotPasswordButton.Layer.BorderWidth = 1;
					forgotPasswordButton.Layer.CornerRadius = 4;
					forgotPasswordButton.Layer.BorderColor = UIColor.Blue.CGColor;
					loginButton.Enabled = false;
				}

				usernameText.Text = "";
				passwordText.Text = "";


			};
				
			registerButton.TouchUpInside += (o, e) => { 

				NavigationController.PushViewController(registerViewController, true);

			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


