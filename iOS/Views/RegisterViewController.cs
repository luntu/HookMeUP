using System.Collections.Generic;
using UIKit;
using Parse;
using ToastIOS;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		
		List<UITextField> fields = new List<UITextField>();
		List<string> values = new List<string>();
			



		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			//NSNotificationCenter.DefaultCenter.AddObserver
			NavigationController.NavigationBarHidden = true;
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			nameText.BecomeFirstResponder();
			SetTags();
			SetFields();
			submitButton.Enabled = true;
			Values();


			for (int i = 0; i < fields.Count-1; i++) {

				fields[i].ShouldReturn += (textField) =>
				{
					
					int nextTag = (int)textField.Tag + 1;
					fields[nextTag].BecomeFirstResponder();

					return true;
				};

			}

		
			usernameTextR.EditingDidEnd += (sender, e) =>
			{
				if (values.Contains(usernameTextR.Text))
				{
					Toast.MakeText("Someone already has that username").Show();
					usernameTextR.Layer.BorderColor = UIColor.Red.CGColor;
					usernameTextR.Layer.BorderWidth = 1;
					usernameTextR.Layer.CornerRadius = 3;
				
					submitButton.Enabled = false;
				}
				else
				{
					submitButton.Enabled = true;
					usernameTextR.Layer.BorderColor = UIColor.Clear.CGColor;
				}
			};



			submitButton.TouchUpInside += (sender, evt) => 
			{
				 
				string name = nameText.Text;
				string surname = surnameText.Text;
				string username = usernameTextR.Text;
				string password = passwordTextR.Text;
				string empNo = emailText.Text;

				if (!name.Equals("") && !surname.Equals("") && !username.Equals("") && !password.Equals("") && !verifyPasswordText.Text.Equals("") && !empNo.Equals(""))
				{
					if (password.Equals(verifyPasswordText.Text))
					{
						verifyPasswordText.Layer.BorderColor = UIColor.Clear.CGColor;
						passwordTextR.Layer.BorderColor = UIColor.Clear.CGColor;
						AddToDB(name, surname, username, password, empNo,23);
						NavigationController.PopViewController(true);
						AlertPopUp("Done!!!", "Registration complete", "OK");
						ClearFields(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,emailText);
						isRegisteredSuccessful = true;
					}
					else 
					{
						ErrorBorder(verifyPasswordText, passwordTextR);
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

			backButtonRegister.TouchUpInside += (obj, evt) => 
			{ 
			
				ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
				NavigationController.PopViewController(true);

			};
		}

		private void ErrorBorder(params UITextField[] textF) 
		{
			foreach (UITextField field in textF) {
				field.Layer.BorderColor = UIColor.Red.CGColor;
				field.Layer.BorderWidth = 1;
				field.Layer.CornerRadius = 3;
			}
		}
		public bool isRegisteredSuccessful { get;set;}
			
		async void AddToDB(string name,string surname, string username, string password,string empNo,int vouchers)
		{
			loadingOverlay = new LoadingOverlay(bounds);
			View.Add(loadingOverlay);
		
			tableNameUserInfo["Name"] = name;
			tableNameUserInfo["Surname"] = surname;
			tableNameUserInfo["Username"] = username;
			tableNameUserInfo["Password"] = password;
			tableNameUserInfo["Email"] = empNo;
			tableNameUserInfo["Vouchers"] = vouchers;

			await tableNameUserInfo.SaveAsync();

			loadingOverlay.Hide();
		}

		void SetTags()
		{
			nameText.Tag = 0;
			surnameText.Tag = 1;
			usernameTextR.Tag = 2;
			passwordTextR.Tag = 3;
			verifyPasswordText.Tag = 4;
			emailText.Tag = 5;

		}

		void SetFields()
		{
			fields.Add(nameText);
			fields.Add(surnameText);
			fields.Add(usernameTextR);
			fields.Add(passwordTextR);
			fields.Add(verifyPasswordText);
			fields.Add(emailText);
		}

		async void Values()
		{

			loadingOverlay = new LoadingOverlay(bounds);
			View.Add(loadingOverlay);

			ParseQuery<ParseObject> query = ParseObject.GetQuery("UserInformation");
			query.Include("Username");

			var coll = await query.FindAsync();

			foreach (ParseObject element in coll)
			{

				values.Add(element.Get<string>("Username"));

			}
			loadingOverlay.Hide();

		}

	}

}


