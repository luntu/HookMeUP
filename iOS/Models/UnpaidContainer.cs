using System.Collections.Generic;
using System.Diagnostics;using System;
namespace HookMeUP.iOS
{
	public class UnpaidContainer
	{
		internal OrdersAdmin Order
		{
			get;
			set;
		}

		Dictionary<string, double> OrdersMap = new Dictionary<string, double>();
		List<string> UnaddedOrders = new List<string>();

		internal void InitialiseMaps()
		{
			try
			{
				OrdersMap.Add(Order.Channel+"-"+Order.PersonOrdered, Order.Price);
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex.Message);
				UnaddedOrders.Add(Order.Channel + "-" + Order.Price);
			}
		}

		internal Dictionary<string, double> GetOrdersMap 
		{
			get
			{
				return OrdersMap;
			}
		}

		internal List<string> GetUnaddedOrders 
		{
			get 
			{
				return UnaddedOrders;
			}
		}
	}
}
