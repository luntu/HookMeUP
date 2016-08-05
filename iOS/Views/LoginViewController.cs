using System;
using System.Collections.Generic;
using Parse;
namespace HookMeUP.iOS
{
	public partial class LoginViewController : ScreenViewController
	{
		

		public override  void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			const string APPLICATION_ID = "G7S25vITx0tfeOhODauYKwtauCvzityLwJFGYHPw";
			const string DOT_NET_ID = "ypPxS2V2rTGl1lNbvEVKUEACKF8PRhWxkWQsbkFe";

			ParseClient.Initialize(APPLICATION_ID, DOT_NET_ID);

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

			loginButton.TouchUpInside += async (sender, evt) =>
		   	{
			   i++;
			 

			   switch (!usernameText.Text.Equals("") && !passwordText.Text.Equals(""))
			   {

				case true:
						   	var query = from userInformation in ParseObject.GetQuery("UserInformation")
						   				where userInformation.Get<string>("Username") == usernameText.Text
						   				select userInformation;

						   	IEnumerable<ParseObject> results = await query.FindAsync();
							foreach (var elements in results)
							{
							   string n = elements.Get<string>("Name");
							   System.Diagnostics.Debug.WriteLine(n);
							}
						/*
						   try
					   {
							

							//	ParseQuery < ParseObject > query = ParseObject.GetQuery("UserInformation");
							   
							string usernameL = usernameText.Text;
							string usernameR = userInfo.Get<string>("Username");


						   	string passwordL = passwordText.Text;
						   	string passwordR = userInfo.Get<string>("Password");

						   if (usernameL.Equals(usernameR) && passwordL.Equals(passwordR))
						   {
							   NavigationScreenController(orderViewController);
							   orderViewController.GetVouchers = userInfo.Get<int>("Vouchers");
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


*/
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


