using System.Collections.Generic;
using System.Diagnostics;
using UIKit;

namespace HookMeUP.iOS
{
	public class ValidateInput
	{
		List<UITextField> textFields = new List<UITextField>();
		List<char> illegalCharacters = new List<char> 
		{
			'.','!', '#', '%', '^', '(', ')', '_', '-', '=', '+', ',', '\'', '/', '?',';',':','\"', ' ' 
		};

		bool IsValid { get; set; } = false;
		UIButton Button { get; set; }

		public ValidateInput(UIButton button, params UITextField[] fields)
		{
			Button = button;
			foreach (UITextField txtFields in fields) 
			{
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
						foreach (char c in illegalCharacters)
						{
							if (f.Text.Contains("" + c))
							{
								hasIllegalCharacter = true;
								break;
							}
						}

						f.EditingDidEnd += (sender, e) =>
						{
							if (!hasIllegalCharacter && (f.Text.Length <= 10 && f.Text.Length >= 5)) IsValid = true;
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
							if ((f.Text.Length >= 5 && f.Text.Length <= 10)) IsValid = true;
							else IsValid = false;
							CheckValidation(IsValid, f);
							ButtonEnable(IsValid);
						};
						break;
						
					case 5:
						f.EditingDidEnd += (sender, e) =>
						{
							if ((f.Text.Length >= 20 && f.Text.Length <= 35)) IsValid = true;
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



		void CheckValidation(bool valid, UITextField f) 
		{
			if (!valid) 
			{ 
				f.Layer.BorderColor = UIColor.Red.CGColor;
				f.Layer.BorderWidth = 1;
				f.Layer.CornerRadius = 3;
			}
			else f.Layer.BorderColor = UIColor.Clear.CGColor;

		}

		void ButtonEnable(bool valid) 
		{
			Button.Enabled = valid;
		}
	}
}

