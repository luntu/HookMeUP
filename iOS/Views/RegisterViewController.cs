using System.Collections.Generic;
using UIKit;
using Parse;
using ToastIOS;
using CoreGraphics;
using System;
using System.IO;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{

		List<UITextField> fields = new List<UITextField>();
		List<string> usernameCheck = new List<string>();
		List<AliensEmployees> employees = new List<AliensEmployees>();
		List<ParseObject> employeeObjs = new List<ParseObject>();
		AuthanticateUser userAuthentication;

		string GetNameTxt { get; set; }
		string GetEmailTxt { get; set; }

		//async void UpdateEmployees()
		//{
		//	LoadingOverlay LO = new LoadingOverlay(bounds);
		//	View.Add(LO);
		//	ParseObject pObj;
		//	var text = File.ReadLines("TempFile/AliensTeam.txt");

		//	foreach (string lines in text)
		//	{
		//		string[] split = lines.Split('@');
		//		string name = split[0];
		//		string domain = name+"@" + split[1];

		//		pObj = new ParseObject("AliensEmployees");
		//		pObj["Name"] = name;
		//		pObj["Email"] = domain;
		//		pObj["IsRegistered"] = false;

		//		await pObj.SaveAsync();

		//	}
		//	LO.Hide();
		//}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			//NSNotificationCenter.DefaultCenter.AddObserver

			NavigationController.NavigationBarHidden = true;
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			nameText.BecomeFirstResponder();

			//UpdateEmployees();

			SetFields();
			submitButton.Enabled = true;
			Values();
			Employees();

			TextFieldKeyboardIteration(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,emailText);

			TextEditing();
			InitializeButtons();
		}

		void InitializeButtons()
		{
			submitButton.TouchUpInside += (sender, evt) =>
			{

				string name = TrimInput(nameText.Text);
				GetNameTxt = name;
				string surname = TrimInput(surnameText.Text);
				string username = TrimInput(usernameTextR.Text);
				string password = TrimInput(passwordTextR.Text);
				string email = TrimInput(emailText.Text);
				GetEmailTxt = email;

				if (!name.Equals("") && !surname.Equals("") && !username.Equals("") && !password.Equals("") && !verifyPasswordText.Text.Equals("") && !email.Equals(""))
				{
					userAuthentication = new AuthanticateUser(employees, name, email);

					if (userAuthentication.IsAlienEmployee())
					{
						if (!userAuthentication.AccountAvailable())
						{
							AddToDB(name, surname, username, password, email, 23);
							NavigationController.PopViewController(true);
							AlertPopUp("Done!!!", "Registration complete", "OK");
							ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
							UpdateRegistered();
						}
						else AlertPopUp("Error","Account already available","Ok");
					}
					else AlertPopUp("Error", "Invalid user", "OK");
				}
				else
				{
					AlertPopUp("Error", "please complete all fields", "OK", "Cancel");
				}

			};

			backButtonRegister.TouchUpInside += (obj, evt) =>
			{

				ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
				NavigationController.PopViewController(true);

			};
		}

		async void UpdateRegistered()
		{
			foreach (ParseObject employeeObjsElements in employeeObjs) 
			{
				string empName = employeeObjsElements.Get<string>("Name");
				string empEmail = employeeObjsElements.Get<string>("Email");

				if (GetNameTxt.Equals(empName) && GetEmailTxt.Equals(empEmail)) 
				{
					employeeObjsElements["IsRegistered"] = true;
					await employeeObjsElements.SaveAsync();
				}
			}
		}

		void TextEditing()
		{
			usernameTextR.EditingDidEnd += (sender, e) =>
			{
				if (usernameCheck.Contains(TrimInput(usernameTextR.Text)))
				{
					Toast.MakeText("Someone already has that username").Show();
					Border(UIColor.Red.CGColor, usernameTextR);

					submitButton.Enabled = false;
				}
				else
				{
					Border(UIColor.Clear.CGColor, usernameTextR);
					submitButton.Enabled = true;

				}
			};

			verifyPasswordText.EditingDidEnd += (sender, e) =>
			{
				if (!TrimInput(passwordTextR.Text).Equals(TrimInput(verifyPasswordText.Text)))
				{
					Toast.MakeText("Username and password don't match").Show();
					Border(UIColor.Red.CGColor, passwordTextR, verifyPasswordText);

					submitButton.Enabled = false;

				}
				else
				{
					Border(UIColor.Clear.CGColor, passwordTextR, verifyPasswordText);
					submitButton.Enabled = true;

				}

			};

			emailText.EditingDidEnd += (sender, e) =>
			{
				if (!TrimInput(emailText.Text).ToLower().Contains("@cowboyaliens.com"))
				{
					Toast.MakeText("Invalid email address").Show();
					Border(UIColor.Red.CGColor, emailText);

					submitButton.Enabled = false;
				}
				else
				{
					Border(UIColor.Clear.CGColor, emailText);
					submitButton.Enabled = true;
				}
			};
		}

		void Border(CGColor color, params UITextField[] textF) 
		{
			foreach (UITextField field in textF) {
				field.Layer.BorderColor = color;
				field.Layer.BorderWidth = 1;
				field.Layer.CornerRadius = 3;
			}
		}

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
			tableNameUserInfo["IsAdmin"] = false;

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
				usernameCheck.Add(element.Get<string>("Username"));
			}
			loadingOverlay.Hide();

		}

		async void Employees()
		{
			LoadingOverlay loadO = new LoadingOverlay(bounds);
			View.Add(loadingOverlay);

			var query = ParseObject.GetQuery("AliensEmployees");
			query.Include("Name").Include("Email").Include("IsRegistered");

			var iEnumerablecoll = await query.FindAsync();

			foreach (var parseObj in iEnumerablecoll) 
			{
				string emplName = parseObj.Get<string>("Name");
				string email = parseObj.Get<string>("Email");
				bool isRegistered = parseObj.Get<bool>("IsRegistered");
				employees.Add(new AliensEmployees(emplName, email, isRegistered));
				employeeObjs.Add(parseObj);
			}

			loadO.Hide();

		}

	}

	class AuthanticateUser // Check if the user is an Alien employee and that he cannot create multiple accounts 
	{
	
		public string Name { get; private set; }
		public string Email { get; private set; }
		public List<AliensEmployees> Employees;
		bool isAvailable = false;

		public AuthanticateUser(List<AliensEmployees>employees, string name, string email) 
		{
			Employees = employees;
			Name = name.ToLower();
			Email = email.ToLower();

		}

		bool Authanticate() 
		{
			foreach (AliensEmployees employeeElements in Employees) 
			{
				string name = employeeElements.Name.ToLower();
				string email = employeeElements.Email.ToLower();
				bool isRegistered = employeeElements.IsRegistered;

				if (name.Equals(Name) && email.Equals(Email))
				{
					if (isRegistered) 
					{
						isAvailable = isRegistered;
					}
					return true;
				}

			}
			return false;
		}

		public bool IsAlienEmployee() 
		{
			if (Authanticate()) return true;
			else return false;
		}


		public bool AccountAvailable()
		{
			return isAvailable;
		}

	}

}


