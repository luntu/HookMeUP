using System;
using Parse;
namespace HookMeUP.iOS
{
	public class User : ParseUser
	{


		string username = "";
		string password = "";
		string email = "";

		async void signUpUserAsync()
		{
			var parseUser = new ParseUser();

			await parseUser.SignUpAsync();
		}
				



	}
	
}




