using System.Collections.Generic;

namespace HookMeUP.iOS
{
	public class OrdersAdmin
	{
		public string ObjectId 
		{
			get;
			private set;
		}
		public string PersonOrdered 
		{
			get;
			private set;
		}

		public List<string> Items 
		{
			get;
			private set;
		}
		public OrdersAdmin(string objectId, string personOrdered, List<string> items)
		{
			ObjectId = objectId;
			PersonOrdered = personOrdered;
			Items = items;
		}
	}
}
