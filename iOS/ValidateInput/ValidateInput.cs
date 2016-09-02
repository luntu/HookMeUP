using System;
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
			'.','!', '#', '%', '^', '(', ')', '_', '-', '=', '+', ',', '\'', '/', '?',';',':','\"'
		};

		public ValidateInput(params UITextField[] fields)
		{
			foreach (UITextField txtFields in fields) 
			{
				textFields.Add(txtFields);
			}
		}

		void ValidateTextInput()
		{
			foreach (var f in textFields) 
			{
				switch (f.Text) 
				{
					case "Name":
						if (f.Text.Length <= 10) IsValid = true;
						else IsValid = false;

						break;
					case "Surname":
						bool hasIllegalCharacter = false;
						foreach (char c in illegalCharacters) 
						{
							if (f.Text.Contains("" + c)){
								hasIllegalCharacter = true;
								break;
							}	
						}
						if (!hasIllegalCharacter && f.Text.Length <=15) 
						{
						}
						break;
					case "Username":
						
						break;
					case "Password":
						
						break;
					case "Verify password":
						
						break;
					case "Email":
						
						break;

					default:
						Debug.WriteLine("No text field found");
						break;
				}
			}
		}

		public bool IsValid { get; private set; } = false;
	}
}

