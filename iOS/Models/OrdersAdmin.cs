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
		public string Channel
		{
			get;
		}

		public double Price 
		{
			get;
		}

		public OrdersAdmin(string objectId, string personOrdered, string channel, double price,List<string> items)
		{
			ObjectId = objectId;
			PersonOrdered = personOrdered;
			Channel = channel;
			Price = price;
			Items = items;
		}

		public OrdersAdmin(string objectId, string personOrdered, string channel, double price) 
		{
			ObjectId = objectId;
			PersonOrdered = personOrdered;
			Channel = channel;
			Price = price;
		}
	}
}
