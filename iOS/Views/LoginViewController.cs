using System;

namespace HookMeUP.iOS
{
	public partial class LoginViewController : ScreenViewController
	{
		

		public override  void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.





			NavigationController.NavigationBarHidden = true;

			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			ShouldReturn(usernameText, passwordText);

			forgotPasswordButton.TouchUpInside += (o, e) =>
			{
				ClearFields(usernameText, passwordText);
				NavigationScreenController(forgotPasswordViewController);
			};

			int i = 0;

			loginButton.TouchUpInside += (o, e) =>
		   {
			   i++;


			   switch (!usernameText.Text.Equals("") && !passwordText.Text.Equals(""))
			   {



				   case true:
					   string[] split = registerViewController.GetValues().Split('#');

					   try
					   {

						   string usernameL = usernameText.Text;
						   string usernameR = split[2];


						   string passwordL = passwordText.Text;
						   string passwordR = split[3];

						   if (usernameL.Equals(usernameR) && passwordL.Equals(passwordR))
						   {
							   NavigationScreenController(orderViewController);
							   orderViewController.GetVouchers = int.Parse(split[5]);
						   }
						   else if (i == 3)
						   {

							   AlertPopUp("Login failed!!", "You failed to login 3 times we suggest \nyou either Register or retrieve lost password", "OK");
							   BorderButton(registerButton, forgotPasswordButton);
							   loginButton.Enabled = false;
						   }
						   else {

							   AlertPopUp("Login failed!!", "Username and/or password is incorrect", "OK");

						   }


					   }
					   catch (IndexOutOfRangeException)
					   {
						   //Do nothing this is an empty value returned. The user did not register
					   }



					   break;

				   case false:

					   AlertPopUp("Error", "Please fill in details", "OK");

					   break;


			   }



			   ClearFields(usernameText, passwordText);

		   };

			registerButton.TouchUpInside += (o, e) =>
			{

				NavigationScreenController(registerViewController);
				ClearFields(usernameText, passwordText);

			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}



		//====================================================


	}
}


