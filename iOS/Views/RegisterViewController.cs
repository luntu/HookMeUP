using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		static List<string> info = new List<string>();

		public string GetValues()  {
			
			string value = "";
			try
			{
				value = info[0];       //throws Argument out of range exception
			}
			catch (ArgumentOutOfRangeException) { AlertPopUp("Invalid account","Account is invalid","OK");}

			return value;
		
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			//NSNotificationCenter.DefaultCenter.AddObserver
			NavigationController.NavigationBarHidden = true;
			 

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

							NavigationController.PopViewController(true);
							AlertPopUp("Done!!!", "Registration complete", "OK");
							ClearFields(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,employeeNoText);

						}
						else { 
						
							AlertPopUp("Registering failed!!!", "passwords do not match", "OK");

							passwordTextR.Highlighted = true;
							verifyPasswordText.Highlighted = true;
						}
						
					}
				else {

					AlertPopUp("Error","please complete all fields","OK","Cancel");

					}

				};

			backButtonRegister.TouchUpInside += (obj, evt) => { 
			
				ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, employeeNoText);
				NavigationController.PopViewController(true);

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


