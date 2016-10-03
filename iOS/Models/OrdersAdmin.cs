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
		public bool Paid
		{
			get;
		}

		public double Price 
		{
			get;
		}

		public OrdersAdmin(string objectId, string personOrdered, string channel, bool paid , double price,List<string> items)
		{
			ObjectId = objectId;
			PersonOrdered = personOrdered;
			Channel = channel;
			Paid = paid;
			Price = price;
			Items = items;
		}
	}
}
