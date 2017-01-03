using System.Collections.Generic;
using UIKit;
using Parse;
using CoreGraphics;
using System;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{

		List<UITextField> fields = new List<UITextField>();
		public static List<string> usernameCheck = new List<string>();
		List<AliensEmployees> employees = new List<AliensEmployees>();
		List<ParseObject> employeeObjs = new List<ParseObject>();
		AuthanticateUser userAuthentication;

		string GetNameTxt
		{
			get;
			set;
		}

		string GetEmailTxt
		{
			get;
			set;
		}

		public override void ViewDidLoad()
		{

			NavigationController.NavigationBarHidden = true;
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			nameText.BecomeFirstResponder();

			//UpdateEmployees();

			SetFields();
			submitButton.Enabled = true;
			LoadUsernames();
			LoadEmployees();
		
			TextFieldKeyboardIteration(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);

			InputTextEditingValidation();
			InitializeButtons();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Border(UIColor.Clear.CGColor, nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
			submitButton.Enabled = true;
		}

		void InputTextEditingValidation()
		{
			ValidateInput validate = new ValidateInput(submitButton, nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
			validate.ValidateTextInput();
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
						Border(UIColor.Clear.CGColor, emailText);

						if (!userAuthentication.AccountAvailable())
						{
							Border(UIColor.Clear.CGColor, usernameTextR);
							UpdateRegistered();
							AddToDB(name, surname, username, password, email, 23);
							NavigationController.PopViewController(true);
							AlertPopUp("Done!!!", "Registration complete", "OK");
							ClearFields(nameText, surnameText, usernameTextR, passwordTextR, verifyPasswordText, emailText);
						}
						else
						{
							Border(UIColor.Red.CGColor, emailText);
							AlertPopUp("Error", "Account already available", "Ok");
						}
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
			LoadingOverlay ldOvly = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(ldOvly);

			foreach (ParseObject employeeObjsElements in employeeObjs)
			{
				string empName = employeeObjsElements.Get<string>("Name");
				string empEmail = employeeObjsElements.Get<string>("Email");

				if (GetNameTxt.ToLower().Equals(empName.ToLower()) && GetEmailTxt.ToLower().Equals(empEmail.ToLower()))
				{
					employeeObjsElements["IsRegistered"] = true;
					await employeeObjsElements.SaveAsync();
				}
			}

			ldOvly.Hide();
		}

		void Border(CGColor color, params UITextField[] textF)
		{
			foreach (UITextField field in textF)
			{
				field.Layer.BorderColor = color;
				field.Layer.BorderWidth = 1;
				field.Layer.CornerRadius = 3;
			}
		}

		async void AddToDB(string name, string surname, string username, string password, string email, int vouchers)
		{
			loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(loadingOverlay);

			var user = new ParseUser()
			{
				Username = username,
				Password = password,
				Email = email
			};
			user["Name"] = name;
			user["Surname"] = surname;
			user["Vouchers"] = vouchers;
			user["IsAdmin"] = false;

			await user.SignUpAsync();

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

		async void LoadUsernames()
		{
			loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(loadingOverlay);
			try
			{
				var query = ParseUser.Query;
				query.Include("username");

				var coll = await query.FindAsync();

				foreach (ParseObject element in coll)
					usernameCheck.Add(element.Get<string>("username"));


			}
			catch (NullReferenceException)
			{
				loadingOverlay.Hide();
				AlertPopUp("Error", "No internet connection", "OK");
			}

			loadingOverlay.Hide();

		}

		async void LoadEmployees()
		{
			LoadingOverlay loadO = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(loadingOverlay);

			try
			{
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

			}
			catch (NullReferenceException ex)
			{
				loadO.Hide();
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			loadO.Hide();

		}

	}

}


