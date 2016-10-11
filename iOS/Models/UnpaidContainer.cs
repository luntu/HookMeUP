using System.Collections.Generic;
using Parse;using System.Diagnostics;using System;
namespace HookMeUP.iOS
{
	public class UnpaidContainer
	{
		Dictionary<string, double> OrdersMap
		{
			get;
			set;
		}

		internal OrdersAdmin Order
		{
			get;
			set;
		}

		string ObjectID 
		{
			get;
		}

		string Name 
		{ 
			get;
		}
		
		string Channel 
		{
			get;
		}
		
		double AmountOwing 
		{
			get;
		}

		double AmountOwingTest 
		{
			get;
			set;
		}

		internal UnpaidContainer() 
		{
			if (Order != null) 
			{
				ObjectID = Order.ObjectId;
				Name = Order.PersonOrdered;
				Channel = Order.Channel;
				AmountOwing = Order.Price;
			}

			if (OrdersMap == null) OrdersMap = new Dictionary<string, double>();
				
		}

		internal void InitialiseMaps()
		{
			try
			{
				OrdersMap.Add(Name, AmountOwingTest);
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex.Message);
				AmountOwingTest += AmountOwing;
			}
		}

		internal Dictionary<string, double> GetOrdersMap 
		{
			get
			{
				return OrdersMap;
			}
		}
	}
}
