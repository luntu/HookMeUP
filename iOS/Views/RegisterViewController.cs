
using Parse;
using UIKit;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{

		void SetTags() {
			nameText.Tag = 0;
			surnameText.Tag = 1;
			usernameTextR.Tag = 2;
			passwordTextR.Tag = 3;
			verifyPasswordText.Tag = 4;
			employeeNoText.Tag = 5;

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

			nameText.ShouldReturn += (textField) => {
				int nextTag = (int)textField.Tag + 1;
				UIResponder nextResponder = ((UITextField)textField).Superview.ViewWithTag(nextTag);
				if (nextResponder.BecomeFirstResponder())
				{
					nextResponder.BecomeFirstResponder();
				}
				else
				{
					((UITextField)textField).ResignFirstResponder();
				}
				return true;
			};
		

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


