using System.Collections.Generic;
using UIKit;
using Parse;
using ToastIOS;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		List<UITextField> fields = new List<UITextField>();
		async void Values() {

			ParseQuery<ParseObject> query = ParseObject.GetQuery("UserInformation");
			query.Include("Username");

			var coll = await query.FindAsync();

			string value;

			foreach (ParseObject element in coll)
			{
				value = element.Get<string>("Username")+"\n";

			}
		
		}

		void SetFields(){
			fields.Add(nameText);
			fields.Add(surnameText);
			fields.Add(usernameTextR);
			fields.Add(passwordTextR);
			fields.Add(verifyPasswordText);
			fields.Add(emailText);
		}

		void SetTags() {
			nameText.Tag = 0;
			surnameText.Tag = 1;
			usernameTextR.Tag = 2;
			passwordTextR.Tag = 3;
			verifyPasswordText.Tag = 4;
			emailText.Tag = 5;

		}
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
		
				//			usernameTextR.AllEvents += async(sender, e) => {
				//				try
				//				{
				//					ParseQuery<ParseObject> query = ParseObject.GetQuery("UserInformation");
				////													where userInfo.Get<string>("Username") == usernameTextR.Text

				////				                                                            select userInfo;
				//					query.Include("Username");
				//					var coll =await query.FindAsync();

				//					foreach (ParseObject element in coll) {
				//						string value = element.Get<string>("Username");
				//						System.Diagnostics.Debug.WriteLine(value);
				//					} 
				//					//ParseObject obj = await query.FirstAsync();
				//					//if(obj.Get<string>("Username").Equals(usernameTextR.Text))
				//					//Toast.MakeText("Someone already has that username ").Show();
				//					submitButton.Enabled = false;
				//				}
				//				catch (ParseException) {
				//					submitButton.Enabled = true;
				//				}

				//			};


				submitButton.TouchUpInside += (sender, evt) => {
							
				string name = nameText.Text;
				string surname = surnameText.Text;
				string username = usernameTextR.Text;
				string password = passwordTextR.Text;
				string empNo = emailText.Text;

				if (!name.Equals("") && !surname.Equals("") && !username.Equals("") && !password.Equals("") && !verifyPasswordText.Text.Equals("") && !empNo.Equals(""))
				{
					if (password.Equals(verifyPasswordText.Text))
					{
						//info.Add(name + "#" + surname + "#" + username + "#" + password + "#" + empNo+"#2");
							
						AddToDB(name, surname, username, password, empNo,23);
						NavigationController.PopViewController(true);
						AlertPopUp("Done!!!", "Registration complete", "OK");
						ClearFields(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,emailText);
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
			
				ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
				NavigationController.PopViewController(true);

			};
		}

		public bool isRegisteredSuccessful { get;set;}
			
		async void AddToDB(string name,string surname, string username, string password,string empNo,int vouchers)
		{

			tableName["Name"] = name;
			tableName["Surname"] = surname;
			tableName["Username"] = username;
			tableName["Password"] = password;
			tableName["Email"] = empNo;
			tableName["Vouchers"] = vouchers;

			await tableName.SaveAsync();
		}




	}




}


