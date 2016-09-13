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

		bool Depleted
		{
			get;
			set;
		}

		bool Negative
		{
			get;
			set;
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

	}
}

