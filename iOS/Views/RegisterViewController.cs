using System;
using System.Collections.Generic;
using Foundation;
using Parse;
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
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			ParseClient.Initialize("G7S25vITx0tfeOhODauYKwtauCvzityLwJFGYHPw", "ypPxS2V2rTGl1lNbvEVKUEACKF8PRhWxkWQsbkFe");

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
							//info.Add(name + "#" + surname + "#" + username + "#" + password + "#" + empNo+"#2");
							

							AddToDB(name, surname, username, password, empNo,23);

							NavigationController.PopViewController(true);
							AlertPopUp("Done!!!", "Registration complete", "OK");
							ClearFields(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,employeeNoText);
							isRegisteredSuccessful = true;

						}
						else { 
						
							AlertPopUp("Registering failed!!!", "passwords do not match", "OK");

							passwordTextR.Highlighted = true;
							verifyPasswordText.Highlighted = true;
							isRegisteredSuccessful = false;
						}
						
					}
				else {

					AlertPopUp("Error","please complete all fields","OK","Cancel");
					isRegisteredSuccessful = false;
					}

				};

			backButtonRegister.TouchUpInside += (obj, evt) => { 
			
				ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, employeeNoText);
				NavigationController.PopViewController(true);

			};
		}

		public bool isRegisteredSuccessful { get;set;}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			RegisterKeyboardNotification();
		}

		void RegisterKeyboardNotification()
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyboardUpNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardDownNotification);
		}

		void KeyboardUpNotification(NSNotification notification)
		{

		}

		void KeyboardDownNotification(NSNotification notification)
		{

		}

		async void AddToDB(string name,string surname, string username, string password,string empNo,int vouchers)
		{
		ParseObject tableName = new ParseObject("UserInformation") 
			{ 
				{ "Name",name },
				{ "Surname" ,surname},
				{ "Username",username},
				{ "Password",password},
				{"EmployeeNumber",empNo },
				{ "Vouchers",vouchers }
			};
		

			await tableName.SaveAsync();
		}

	}
}


