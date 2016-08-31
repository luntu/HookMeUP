using System.Collections.Generic;
using UIKit;
using Parse;
using ToastIOS;
using CoreGraphics;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		
		List<UITextField> fields = new List<UITextField>();
		List<string> usernameCheck = new List<string>();


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			//NSNotificationCenter.DefaultCenter.AddObserver

			NavigationController.NavigationBarHidden = true;
			DismissKeyboardOnBackgroundTap();
			RegisterForKeyboardNotifications();
			nameText.BecomeFirstResponder();
			UpdateOrders();

			SetFields();
			submitButton.Enabled = true;
			Values();

			TextFieldKeyboardIteration(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,emailText);
		
		
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
					Border(UIColor.Red.CGColor,passwordTextR, verifyPasswordText);

					submitButton.Enabled = false;

				}
				else 
				{
					Border(UIColor.Clear.CGColor,passwordTextR,verifyPasswordText);	
					submitButton.Enabled = true;

				}

			};


			submitButton.TouchUpInside += (sender, evt) => 
			{
				 
				string name = TrimInput(nameText.Text);
				string surname = TrimInput(surnameText.Text);
				string username = TrimInput(usernameTextR.Text);
				string password = TrimInput(passwordTextR.Text);
				string empNo = TrimInput(emailText.Text);

				if (!name.Equals("") && !surname.Equals("") && !username.Equals("") && !password.Equals("") && !verifyPasswordText.Text.Equals("") && !empNo.Equals(""))
				{
					
						AddToDB(name, surname, username, password, empNo,23);
						NavigationController.PopViewController(true);
						AlertPopUp("Done!!!", "Registration complete", "OK");
						ClearFields(nameText,surnameText,usernameTextR,passwordTextR,verifyPasswordText,emailText);
						isRegisteredSuccessful = true;
				
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

		void Border(CGColor color, params UITextField[] textF) 
		{
			foreach (UITextField field in textF) {
				field.Layer.BorderColor = color;
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





		async void UpdateOrders()
		{

			List<string> arr = new List<string>
			{"Espresso#15,00",
			"Red Espresso#15,50",
			"Cappuccino#19,00",
			"Red Cappuccino#19,50",
			"Vanilla Cappuccino#28,00",
			"Hazelnut Cappuccino#28,00",
			"Latte#22,50",
			"Red Latte#20,00",
			"Vanilla Latte#30,00",
			"Hazelnut Latte#30,00",
			"Cafe Americano#18,50",
			"Cafe Mocha#24,50",
			"Hot Chocolate#20,00"};

			foreach (string e in arr)
			{
				ParseObject pObj = new ParseObject("Coffees");
				string[] split = e.Split('#');
				pObj["Title"] = split[0];
				pObj["Price"] = double.Parse(split[1]);

				await pObj.SaveAsync();
			}
		}

	}



}


