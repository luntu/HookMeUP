using System.Collections.Generic;
using System.Diagnostics;
using UIKit;

namespace HookMeUP.iOS
{
	public class ValidateInput
	{
		RegisterViewController register = new RegisterViewController();
		List<UITextField> textFields = new List<UITextField>();
		List<char> IllegalCharacters = new List<char> 
		{
			'.', '!', '#', '%', '^', '(', ')', '_', '-', '=',
			'+', ',', '\'', '/', '?', ';', ':', '\"','$', '±',
			'*', '>', '<', '[', ']', '{', '}' , '`', '~'
		};

		bool IsValid 
		{
			get;
			set;
		} = false;

		bool isUsernameUsed 
		{
			get;
			set;
		}
		UIButton Button 
		{
			get;
			set;
		}
		bool passwordMatches
		{
			get;
			set;
		}
		UITextField PasswordField
		{
			get;
			set;
		}

		const int PASSWORD_FIELD = 3;

		public ValidateInput(UIButton button, params UITextField[] fields)
		{
			Button = button;
			foreach (UITextField txtFields in fields) 
			{
				if (txtFields.Tag == 3) PasswordField = txtFields;
				
				textFields.Add(txtFields);
			}
		}

		public void ValidateTextInput()
		{
			bool hasIllegalCharacter = false;

			foreach (var f in textFields) 
			{
				switch (f.Tag) 
				{
					case 0:                                //name length < 10
						f.EditingDidEnd += (sender, e) => 
						{
							if (f.Text.Length <= 10) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f);
							ButtonEnable(IsValid);
						};

						break;
						
					case 1:                             //Surname lenght < 15
						f.EditingDidEnd += (sender, e) =>
						{
							if (f.Text.Length <= 15) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f);
							ButtonEnable(IsValid);
						};
						break;
						
					case 2:                            //Enter text of length >= 5 and <=10 and no illegal characters
											
						f.EditingDidEnd += (sender, e) =>
						{
							foreach (char c in IllegalCharacters)
							{
								if (f.Text.Contains("" + c))
								{
									hasIllegalCharacter = true;
									break;
								}
							}
							
							isUsernameUsed = false;
							foreach (string usernameElements in RegisterViewController.usernameCheck)
							{
					
								if (usernameElements.ToLower().Equals(register.TrimInput(f.Text.ToLower())))
								{
									isUsernameUsed = true;
									break;
								}
							}

							if (!hasIllegalCharacter && (f.Text.Length <= 10 && f.Text.Length >= 5) && !isUsernameUsed) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f);
							ButtonEnable(IsValid);
						};
						break;
						
					case 3:                            //Password can have illegal character but its length must be between 5 and 10 inclusive
						f.EditingDidEnd += (sender, e) =>
						{
							if ((f.Text.Length >= 5 && f.Text.Length <= 10)) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f);
							ButtonEnable(IsValid);
						};
						break;

					case 4:
						f.EditingDidEnd += (sender, e) =>
						{
							
								if (register.TrimInput(PasswordField.Text.ToLower()).Equals(f.Text.ToLower())) passwordMatches = true;
								else passwordMatches = false;
							
							
							if ((f.Text.Length >= 5 && f.Text.Length <= 10) && passwordMatches) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f ,PasswordField);
							ButtonEnable(IsValid);
						};
						break;
						
					case 5:
						f.EditingDidEnd += (sender, e) =>
						{
							if (f.Text.Equals(string.Empty)) f.Layer.BorderColor = UIColor.Clear.CGColor;
							if ((f.Text.Length >= 20 && f.Text.Length <= 35) && register.TrimInput(f.Text.ToLower()).Contains("@cowboyaliens.com")) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f);
							ButtonEnable(IsValid);
						};
						break;

					default:
						Debug.WriteLine("No text field found");
						break;
				}
			}
		}

		void CheckValidation(bool valid, params UITextField[] fields) 
		{
			foreach (UITextField f in fields) 
			{
				if (!valid)
				{
					f.Layer.BorderColor = UIColor.Red.CGColor;
					f.Layer.BorderWidth = 1;
					f.Layer.CornerRadius = 3;
				}
				else f.Layer.BorderColor = UIColor.Clear.CGColor;
			}


		}

		void ButtonEnable(bool valid) 
		{
			Button.Enabled = valid;
		}

	}
}

