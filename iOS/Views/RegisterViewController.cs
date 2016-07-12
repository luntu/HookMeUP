using System.Collections.Generic;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		static List<string> info = new List<string>();
		UIAlertView alert = new UIAlertView();

		public string getValues()  {
			string value = "";
			try
			{
				 value =  info[0];
			}
			catch (System.ArgumentOutOfRangeException){

				alert.Title = "Login failed";
				alert.Message = "Username and/or password is incorrect";
				alert.AddButton("OK");
				alert.Show();

			}
			return value;
		
		}

	//	 static LoginViewController loginViewController = new LoginViewController();




	


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			 

			submitButton.TouchUpInside += (sender, evt) => {



					string name = nameText.Text;
					string surname = surnameText.Text;
					string username = usernameTextR.Text;
					string password = passwordTextR.Text;
					string empNo = employeeNoText.Text;

					if (!name.Equals("") && !surname.Equals("") && !username.Equals("") && !password.Equals("") && !verifyPasswordText.Text.Equals("") && !empNo.Equals(""))
					{
						if (password.Equals(verifyPasswordText.Text))
						{
							info.Add(name + "#" + surname + "#" + username + "#" + password + "#" + empNo);
							alert.Title = "Done!!!";
							alert.AddButton("OK");
							alert.Message = "Registration complete";
							alert.Show();
							NavigationController.PushViewController(loginViewController,true);
						}
						else { 
						
							alert.Title = "Registering failed!!!";
							alert.AddButton("OK");
							alert.Message = "passwords do not match";
							alert.Show();
							passwordTextR.Highlighted = true;
							verifyPasswordText.Highlighted = true;
						}
						
					}
				else {
					alert.Title = "Error";
					alert.AddButton("OK");
					alert.Message = "please complete all fields";
					alert.Show();
					}
					nameText.Text = "";
					surnameText.Text = "";
					usernameTextR.Text = "";
					passwordTextR.Text = "";
					verifyPasswordText.Text = "";
					employeeNoText.Text = "";
				};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		/*	void AlertMessage() {

		
			alert.Title = "Error";
			alert.AddButton("OK");
			alert.Message = "please complete all fields";
			/*alert.Clicked += (p, evt) =>
			{
				int button = (int)evt.ButtonIndex;
				if (button == 0)
				{
					Debug.WriteLine("Ok button clicked");
				}
				else if (button == 1)
				{
					Debug.WriteLine("Cancel button clicked");
				}
			};* /
			alert.Show();
		}*/


	}
}


