namespace HookMeUP.iOS
{
	public class AliensEmployees
	{
		public string Name { get; private set; }
		public string Email { get; private set; }
		public bool IsRegistered { get; private set; }

		public AliensEmployees(string name, string email, bool isRegistered)
		{
			Name = name;
			Email = email;
			IsRegistered = isRegistered;
		}
	}
}

