using System;
using System.Diagnostics;
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class LoginViewController : ScreenViewController
	{


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			NavigationController.NavigationBarHidden = true;

			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();

			usernameText.BecomeFirstResponder();
			ShouldReturn(usernameText, passwordText);
			TextFieldKeyboardIteration(usernameText, passwordText);

			forgotPasswordButton.TouchUpInside += (o, e) =>
			{
				ClearFields(usernameText, passwordText);
				NavigationScreenController(forgotPasswordViewController);
			};

			int i = 0;

			loginButton.TouchUpInside += async (sender, evt) =>
		   	{
				   i++;
				string username = usernameText.Text;
				string password = passwordText.Text;

				switch ((!username.Equals("") && !password.Equals("")) && (!(username.Length > 10) && !(username.Length < 5)))
				   {

					   case true:
						   ParseUser result = null;

						   try
						   {
							   loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
							   View.Add(loadingOverlay);

							   result = await ParseUser.LogInAsync(TrimInput(usernameText.Text), TrimInput(passwordText.Text));
						   }
						   catch (ParseException ex)
						   {
							   Debug.WriteLine(ex.GetType());
							   loadingOverlay.Hide();
							   AlertPopUp("Login failed", "Username or password incorrect", "OK");

							   if (i == 3)
							   {
								   AlertPopUp("Login failed!!", "You failed to login 3 times we suggest \nyou either Register or retrieve lost password", "OK");
								   loginButton.Enabled = false;
							   }
						   }
						   catch (NullReferenceException)
						   {
							   loadingOverlay.Hide();
							   AlertPopUp("Login failed!!", "Please check your internet connection", "OK");
						}

						   try
						   {
							   string name = result.Get<string>("Name");
							   string surname = result.Get<string>("Surname");
							   bool isAdmin = result.Get<bool>("IsAdmin");
							   int vouchers = result.Get<int>("Vouchers");
							   string userChannelName = name + surname;

							   orderViewController.GetName = name;
							   orderViewController.GetSurname = surname;
							   orderViewController.CurrentUser = result;
							   orderViewController.GetVouchers = vouchers;

							   orderViewController.GetUserChannelName = userChannelName;
							   // create user channel
							   var installation = ParseInstallation.CurrentInstallation;
							   //Debug.WriteLine( installation.DeviceToken);

							   if (isAdmin)
							   {
								   installation.Channels = new string[] { "Admin" };
							   }
							   else
							   {
								   installation.Channels = new string[] { userChannelName.ToLower() };
							   }

							   await installation.SaveAsync();

							   loadingOverlay.Hide();

							   if (isAdmin) NavigationScreenController(adminViewController);
							   else NavigationScreenController(orderViewController);

						   }
						   catch (ParseException ex)
						   {
							   loadingOverlay.Hide();
							   Debug.WriteLine(ex.Message);
							   AlertPopUp("Error", "Can't login server error, try again in a while ", "Ok");
						   }
							catch (NullReferenceException e)
						   {
							Debug.WriteLine(e.Message);
							}

						   break;

					   case false:

							if( username.Length == 0 || password.Length == 0)AlertPopUp("Error", "Please fill in all your credentials", "OK");
							else if ((username.Length > 10 || username.Length < 5)) AlertPopUp("Error", "Username or password is too long/short", "OK");

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
	}

}


