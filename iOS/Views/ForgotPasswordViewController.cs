using System;
using MessageUI;
using Parse;

namespace HookMeUP.iOS
{
	public partial class ForgotPasswordViewController : ScreenViewController
	{

		private MFMailComposeViewController mailcontroller;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();


			int i = 0;

			getPasswordButton.TouchUpInside += async(obj, evt) => {
				
				i++;

				switch (!usernameTextForgot.Text.Equals("") && !emailTextForgot.Text.Equals("")) {

					case true:
						
						try
						{
							loadingOverlay = new LoadingOverlay(bounds);
							View.Add(loadingOverlay);

							ParseQuery<ParseObject> query = from userInfo in ParseObject.GetQuery("UserInformation")
															where userInfo.Get<string>("Username") == usernameTextForgot.Text
															&& userInfo.Get<string>("Email") == emailTextForgot.Text
															select userInfo;
							
							ParseObject objQ = await query.FirstAsync();

							string name = objQ.Get<string>("Name");
							string email = objQ.Get<string>("Email");
							string password = objQ.Get<string>("Password");

							loadingOverlay.Hide();

							if (MFMailComposeViewController.CanSendMail)
							{
								mailcontroller = new MFMailComposeViewController();
								mailcontroller.SetToRecipients(new string[] { email });
								mailcontroller.SetSubject("no reply");
								mailcontroller.SetMessageBody("Hi\b " + name + "\nWe found your lost password\nPassword: " + password + "\nRegards\nHookMeUP Team", false);
							
								mailcontroller.Finished += (sender, e) =>
								{
									Console.WriteLine(e.Result.ToString());
									e.Controller.DismissViewController(true, null);
								};
								PresentViewController(mailcontroller, true, null);
								AlertPopUp("Done!", "We,ve sent an email to " + email + "\nIt will take a while", "OK");

							}
							else System.Diagnostics.Debug.WriteLine("Mail can't be sent");

						}
						catch (ParseException){
							AlertPopUp("Error", "Username and email do not match", "OK");
							if (i == 3)
							{
								AlertPopUp("Error", "You failed to retrive password 3 times \n we suggest you register as a new user ", "OK");
								ClearFields(emailTextForgot, usernameTextForgot);
								NavigationController.PopViewController(true);
							}
						}
						break;

					case false:
						
						AlertPopUp("Error","Please fill in details","Ok");

						break;
						//no default
				}
			
			};

			backButtonForgot.TouchUpInside += (o, e) =>
			{
				NavigationController.PopViewController(true);
				ClearFields(usernameTextForgot,emailTextForgot);
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


