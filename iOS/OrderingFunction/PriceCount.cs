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

		public bool Depleted
		{
			get;
			private set;
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


		public void PriceChange() 
		{
			if (Selected) ReturnPrice += Price;
			if (Deselected && !Depleted) ReturnPrice -= Price;

			if (ReturnPrice <= 0) Depleted = true;
			else Depleted = false;
		}

		public double GetPrice()
		{
			return ReturnPrice;
		}
	}
}

