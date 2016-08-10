using System.Collections.Generic;
using UIKit;
using Parse;
using ToastIOS;
using System;
using CoreGraphics;

namespace HookMeUP.iOS
{
	public partial class RegisterViewController : ScreenViewController
	{
		LoadingOverlay loadingOverlay;
		List<UITextField> fields = new List<UITextField>();
		string values;
	
		async void Values() {

			var bounds = UIScreen.MainScreen.Bounds;
			loadingOverlay = new LoadingOverlay(bounds);
			View.Add(loadingOverlay);

			ParseQuery<ParseObject> query = ParseObject.GetQuery("UserInformation");
			query.Include("Username");

			var coll = await query.FindAsync();



			foreach (ParseObject element in coll)
			{
				values = element.Get<string>("Username")+"\n";

			}
			loadingOverlay.Hide();
		
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
		
				usernameTextR.TouchCancel += (sender, e) => {

					
								
				};


				submitButton.TouchUpInside += (sender, evt) => {

					if (values.Contains(usernameTextR.Text))
					{
					Toast.MakeText("Username already available").Show();
					}
					else Toast.MakeText("none").Show();

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

	public class LoadingOverlay : UIView
	{
		// control declarations
		UIActivityIndicatorView activitySpinner;
		UILabel loadingLabel;

		public LoadingOverlay(CGRect frame) : base(frame)
		{
			// configurable bits

			Alpha = 0.75f;
			AutoresizingMask = UIViewAutoresizing.All;

			nfloat labelHeight = 22;
			nfloat labelWidth = Frame.Width - 20;

			// derive the center x and y
			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;

			// create the activity spinner, center it horizontall and put it 5 points above center x
			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new CGRect(
				centerX - (activitySpinner.Frame.Width / 2),
				centerY - activitySpinner.Frame.Height - 20,
				activitySpinner.Frame.Width,
				activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
			AddSubview(activitySpinner);
			activitySpinner.StartAnimating();
			activitySpinner.Color = UIColor.Black;
			// create and configure the "Loading Data" label
			loadingLabel = new UILabel(new CGRect(
				centerX - (labelWidth / 2),
				centerY,
				labelWidth,
				labelHeight
				));
			loadingLabel.BackgroundColor = UIColor.Clear;
			loadingLabel.TextColor = UIColor.Black;
			loadingLabel.Text = "Loading Data...";
			loadingLabel.TextAlignment = UITextAlignment.Center;
			loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
			AddSubview(loadingLabel);

		}

		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide()
		{
			UIView.Animate(
				0.5, // duration
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); }
			);
		}
	}


}


