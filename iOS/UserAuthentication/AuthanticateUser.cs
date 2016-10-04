using System.Collections.Generic;
using HookMeUP.iOS;

class AuthanticateUser // Check if the user is an Alien employee and that he cannot create multiple accounts 
{

	public string Name
	{
		get;
	}

	public string Email
	{
		get;
	}

	public List<AliensEmployees> Employees
	{
		get;
	}

	bool isAvailable
	{
		get;
		set;
	}

	public AuthanticateUser(List<AliensEmployees> employees, string name, string email)
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
			System.Diagnostics.Debug.WriteLine(name + "\t" + Name + "\n" + email + "\t" + Email);
			if (name.Equals(Name) && email.Equals(Email))
			{
				if (isRegistered)
					isAvailable = isRegistered;
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
