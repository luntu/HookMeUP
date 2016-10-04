using System;
using System.Diagnostics;
using Parse;
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

				   switch (!usernameText.Text.Equals("") && !passwordText.Text.Equals(""))
				   {

					   case true:
						   ParseUser result = null;

						   try
						   {
							   loadingOverlay = new LoadingOverlay(bounds);
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

						   try
						   {
							   string name = result.Get<string>("Name");
							   string surname = result.Get<string>("Surname");
							   bool isAdmin = result.Get<bool>("IsAdmin");
							   int vouchers = result.Get<int>("Vouchers");
							   string userChannelName = name+surname;
							  
							   orderViewController.GetName = name;
							   orderViewController.GetSurname = surname;
							   orderViewController.CurrentUser = result;
							   orderViewController.GetVouchers = vouchers;
							   
							   orderViewController.GetUserChannelName = userChannelName;
							   // create user channel
							   var installation = ParseInstallation.CurrentInstallation;
								
							   Debug.WriteLine(isAdmin);

							   if (isAdmin) installation.Channels = new string[] { "Admin" };
							   else installation.Channels = new string[] { userChannelName.ToLower() };

							   await installation.SaveAsync();

							   loadingOverlay.Hide();

							   if (isAdmin) NavigationScreenController(adminViewController);
							   else NavigationScreenController(orderViewController);

						   }
						   catch (NullReferenceException ex)
						   {
							   Debug.WriteLine(ex.Message);
						   }
						   catch (ParseException ex)
						   {
							   Debug.WriteLine(ex.Message);
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
	}

}


