using System.Diagnostics;
using UIKit;

namespace HookMeUP.iOS
{
	class PriceCount
	{
		public double Price
		{
			get;
			set;
		}

		double ReturnPrice
		{
			get;
			set;
		}

		public bool Selected
		{
			get;
			set;
		}

		public bool Deselected
		{
			get;
			set;
		}

		public bool Depleted
		{
			get;
			private set;
		}

		int NegativeVouchers
		{
			get;
			set;
		}
		UITextField CostText
		{
			get;
			set;
		}

		public PriceCount(UITextField costText)
		{
			CostText = costText;
		}

		public void PriceChange() 
		{
			if (Selected) ReturnPrice += Price;
			if (Deselected) ReturnPrice -= Price;

			if (ReturnPrice <= 0) Depleted = true;
			else Depleted = false;
			Debug.WriteLine(ReturnPrice+"\n"+Depleted);
		}

		public double GetPrice()
		{
			return ReturnPrice;
		}
	}
}

