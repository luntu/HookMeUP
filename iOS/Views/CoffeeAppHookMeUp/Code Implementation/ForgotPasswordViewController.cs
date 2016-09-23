using Parse;
using System.Diagnostics;

namespace HookMeUP.iOS 
{
	public partial class ForgotPasswordViewController : ScreenViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			emailTextForgot.BecomeFirstResponder();
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			ShouldReturn(emailTextForgot);


			getPasswordButton.TouchUpInside += async (object sender, System.EventArgs e) =>
			{ 
				try
				{
					string email = emailTextForgot.Text;
					await ParseUser.RequestPasswordResetAsync(email);
					AlertPopUp("Link sent","A password reset link has been sent to "+email, "OK");
				}
				catch(ParseException ex) 
				{
					Debug.WriteLine(ex.Message);
				}
			
			};

			backButtonForgot.TouchUpInside += (o, e) =>
			{
				NavigationController.PopViewController(true);
				ClearFields(emailTextForgot);
			};
		}

	}

}