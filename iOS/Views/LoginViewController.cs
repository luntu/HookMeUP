using System;
using System.Diagnostics;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class LoginViewController : ScreenViewController
	{

	

		//static RegisterViewController registerViewController = new RegisterViewController();


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			  
			forgotPasswordButton.TouchUpInside += (o, e) => { /*forgot password page*/ 

			};

			int i = 0;

			loginButton.TouchUpInside += (o, e) => {
				i++;

				string[] split = registerViewController.GetValues().Split('#');
				try
				{
					string usernameR = split[2];
					string usernameL = usernameText.Text;

					string passwordR = split[3];
					string passwordL = passwordText.Text;

					//try{
					if (usernameL.Equals(usernameR) && passwordL.Equals(passwordR))
					{
						NavigationScreenController(registerViewController);
					}
					else {

						AlertPopUp("Login failed", "Username and/or password is incorrect", "OK");
					}

					//}catch (ArgumentOutOfRangeException ) {

					//	AlertPopUp("Login failed","Username and/or password is incorrect","OK");

					//}
				}
				catch (IndexOutOfRangeException) {
					AlertPopUp("Login failed","Fill in all the details","OK");
				}
				if (i == 3) {


					AlertPopUp("Login failed","You have entered the password 3 times incorrectly\n we suggest you click the 'forgot password button' or 'Register button'","OK");


					BorderButton(forgotPasswordButton, registerButton);
					loginButton.Enabled = false;
				}

				usernameText.Text = "";
				passwordText.Text = "";


			};
				


			registerButton.TouchUpInside += (o, e) => {

				NavigationScreenController(registerViewController);

			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


