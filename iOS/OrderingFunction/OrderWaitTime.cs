using System;
namespace HookMeUP.iOS
{
	class OrderWaitTime
	{

		const int AVERAGE_ORDER_TIME = 2;

		public int GetOrdersTotal { get; set; }

		public int CalculateWaitTime()
		{
			int time = GetOrdersTotal * AVERAGE_ORDER_TIME;
			return time;
		}

	}
}

