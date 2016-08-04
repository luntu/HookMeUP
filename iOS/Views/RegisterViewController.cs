
namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			//NSNotificationCenter.DefaultCenter.AddObserver
			NavigationController.NavigationBarHidden = true;
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();


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


