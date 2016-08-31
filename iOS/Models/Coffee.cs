namespace HookMeUP.iOS
{
	public class Coffee
	{
		public Coffee(string title, string imageName, string price)
		{
			Title = title;
			ImageName = imageName;
			Price = price;
		}

		public string Title { get; private set; }
		public string ImageName { get; private set; }
		public string Price { get; private set; }
	}
}

