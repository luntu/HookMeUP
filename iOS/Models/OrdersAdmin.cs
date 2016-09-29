using System.Collections.Generic;

namespace HookMeUP.iOS
{
	public class OrdersAdmin
	{
		public string ObjectId 
		{
			get;
		}
		public string PersonOrdered 
		{
			get;
		}

		public List<string> Items 
		{
			get;
		}
		public OrdersAdmin(string objectId, string personOrdered, List<string> items)
		{
			ObjectId = objectId;
			PersonOrdered = personOrdered;
			Items = items;
		}
	}
}
