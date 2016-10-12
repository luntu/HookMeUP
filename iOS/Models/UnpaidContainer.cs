using System.Collections.Generic;
using System.Diagnostics;using System;
using Parse;
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
		List<ParseObject> ObjectParse = new List<Parse.ParseObject>();
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

		internal void InitialiseList(ParseObject pObjectParse) 
		{
			try 
			{
				ObjectParse.Add(pObjectParse);
			} 
			catch (Exception e) 
			{
				Debug.WriteLine(e.GetType() + "\n" + e.Message);
			}
		}

		internal Dictionary<string, double> GetOrdersMap 
		{
			get
			{
				return OrdersMap;
			}
		}

		internal List<ParseObject> GetParseObjectList 
		{
			get 
			{
				return ObjectParse;
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
